// <copyright file="AddEventCommandHandler.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Features.Events.AddEvent
{
    using AutoMapper;
    using EventManagementAPI.Core.Application.Contracts.Identity;
    using EventManagementAPI.Core.Application.Contracts.Messaging.Commands;
    using EventManagementAPI.Core.Application.Contracts.Persistence;
    using EventManagementAPI.Core.Application.Extensions;
    using EventManagementAPI.Core.Application.Response;
    using EventManagementAPI.Core.Domain.Entities;
    using EventManagementAPI.Core.Domain.Errors;

    public class AddEventCommandHandler : ICommandHandler<AddEventCommand, Result<string>>
    {
        private readonly IRepository<Event> eventRepository;
        private readonly IUserService userService;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public AddEventCommandHandler(IRepository<Event> eventRepository, IUserService userService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.eventRepository = eventRepository;
            this.userService = userService;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<Result<string>> Handle(AddEventCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var userIdString = request.User.GetUserId();
                if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
                {
                    return Result<string>.Failure(DomainErrors.Auth.NotAuthenticated());
                }

                request.CreatedBy = userId;

                var user = await this.userService.GetUserDetailsByIdAsync(request.CreatedBy);
                if (user == null)
                {
                    return Result<string>.Failure(DomainErrors.Auth.UserNotFound(request.CreatedBy));
                }

                var eventRequest = this.mapper.Map<Event>(request);
                await this.eventRepository.AddAsync(eventRequest);

                await this.unitOfWork.SaveChangesAsync(cancellationToken);
                await this.unitOfWork.CommitAsync();

                return Result<string>.Success("Event added successfully !");
            }
            catch (Exception)
            {
                await this.unitOfWork.RollbackAsync();
                return Result<string>.Failure(DomainErrors.Transaction.TransactionFailed());
            }
        }
    }
}
