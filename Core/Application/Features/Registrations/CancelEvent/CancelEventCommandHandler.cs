// <copyright file="CancelEventCommandHandler.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Features.Registrations.CancelEvent
{
    using AutoMapper;
    using EventManagementAPI.Core.Application.Contracts.Messaging.Commands;
    using EventManagementAPI.Core.Application.Contracts.Persistence;
    using EventManagementAPI.Core.Application.Extensions;
    using EventManagementAPI.Core.Application.Response;
    using EventManagementAPI.Core.Domain.Entities;
    using EventManagementAPI.Core.Domain.Errors;

    public class CancelEventCommandHandler : ICommandHandler<CancelEventCommand, Result<string>>
    {
        private readonly IRepository<Registration> registrationRepository;
        private readonly IRepository<Event> eventRepository;
        private readonly IUnitOfWork unitOfWork;

        public CancelEventCommandHandler(IRepository<Registration> registrationRepository, IRepository<Event> eventRepository, IUnitOfWork unitOfWork)
        {
            this.registrationRepository = registrationRepository;
            this.eventRepository = eventRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<string>> Handle(CancelEventCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var userIdString = request.User!.GetUserId();
                if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
                {
                    return Result<string>.Failure(DomainErrors.Auth.NotAuthenticated());
                }

                request.UserId = userId;

                await this.unitOfWork.BeginTransactionAsync();
                var registration = await this.registrationRepository.FindFirstOrDefaultAsync(x =>
            x.EventId == request.EventId && x.UserId == userId && x.RegisterType == Domain.Enums.RegisterType.REGISTERED);

                var evt = await this.eventRepository.GetByIdAsync(request.EventId);
                if (registration == null)
                {
                    return Result<string>.Failure(DomainErrors.Registration.NotFoundForEvent(request.EventId));
                }

                if (evt!.StartDateTime < DateTime.UtcNow)
                {
                    return Result<string>.Failure(DomainErrors.Event.EventAlreadyPassed());
                }

                evt!.TotalRegistrations--;

                if (registration.RegisterType == Domain.Enums.RegisterType.CANCELED)
                {
                    return Result<string>.Failure(DomainErrors.Registration.AlreadyCanceled());
                }

                registration.RegisterType = Domain.Enums.RegisterType.CANCELED;

                await this.registrationRepository.UpdateAsync(registration);
                await this.eventRepository.UpdateAsync(evt);

                await this.unitOfWork.SaveChangesAsync(cancellationToken);
                await this.unitOfWork.CommitAsync();
                return Result<string>.Success("Event canceled successfully!");
            }
            catch (Exception e)
            {
                await this.unitOfWork.RollbackAsync();
                return Result<string>.Success(e.Message);
                //return Result<string>.Failure(DomainErrors.Transaction.TransactionFailed());
            }
         }
    }
}
