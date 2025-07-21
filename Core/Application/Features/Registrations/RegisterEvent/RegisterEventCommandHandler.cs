// <copyright file="RegisterEventCommandHandler.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Features.Registrations.RegisterEvent
{
    using AutoMapper;
    using EventManagementAPI.Core.Application.Contracts.Messaging.Commands;
    using EventManagementAPI.Core.Application.Contracts.Persistence;
    using EventManagementAPI.Core.Application.DTO;
    using EventManagementAPI.Core.Application.Extensions;
    using EventManagementAPI.Core.Application.Response;
    using EventManagementAPI.Core.Domain.Entities;
    using EventManagementAPI.Core.Domain.Enums;
    using EventManagementAPI.Core.Domain.Errors;

    public class RegisterEventCommandHandler : ICommandHandler<RegisterEventCommand, Result<string>>
    {
        private readonly IRepository<Event> eventRepository;
        private readonly IRepository<Registration> registrationRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public RegisterEventCommandHandler(IRepository<Event> eventRepository, IRepository<Registration> registrationRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.eventRepository = eventRepository;
            this.registrationRepository = registrationRepository;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<Result<string>> Handle(RegisterEventCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var userIdString = request.User.GetUserId();
                if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
                {
                    return Result<string>.Failure(DomainErrors.Auth.NotAuthenticated());
                }

                request.UserId = userId;

                await this.unitOfWork.BeginTransactionAsync();

                var evt = await this.eventRepository.GetByIdAsync(request.EventId);
                if (evt is null)
                {
                    return Result<string>.Failure(DomainErrors.Event.NotFound(request.EventId));
                }

                var allRegistrations = await this.registrationRepository.FindAllAsync(r =>
                    r.EventId == request.EventId &&
                    r.RegisterType == RegisterType.REGISTERED);

                if (allRegistrations.Count() >= evt.Capacity)
                {
                    return Result<string>.Failure(DomainErrors.Registration.EventIsFull(evt.Title!));
                }

                if (DateTime.UtcNow > evt.StartDateTime)
                {
                    return Result<string>.Failure(DomainErrors.Registration.CutoffPassed(evt.Title!));
                }

                var allMatching = await this.registrationRepository.FindAllAsync(r =>
                    r.EventId == request.EventId &&
                    r.UserId == request.UserId &&
                    r.RegisterType == RegisterType.REGISTERED);

                if (allMatching.Any())
                {
                    return Result<string>.Failure(DomainErrors.Registration.AlreadyRegistered(evt.Title!));
                }

                var registration = this.mapper.Map<Registration>(request);
                await this.registrationRepository.AddAsync(registration);

                await this.unitOfWork.SaveChangesAsync(cancellationToken);
                await this.unitOfWork.CommitAsync();

                return Result<string>.Success("Registered successfully.");
            }
            catch (Exception e)
            {
                await this.unitOfWork.RollbackAsync();
                return Result<string>.Failure(DomainErrors.Transaction.TransactionFailed());
            }
        }
    }
}
