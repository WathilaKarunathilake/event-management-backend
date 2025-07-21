// <copyright file="DeleteEventCommandHandler.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Features.Events.DeleteEvent
{
    using EventManagementAPI.Core.Application.Contracts.Identity;
    using EventManagementAPI.Core.Application.Contracts.Messaging.Commands;
    using EventManagementAPI.Core.Application.Contracts.Persistence;
    using EventManagementAPI.Core.Application.Extensions;
    using EventManagementAPI.Core.Application.Response;
    using EventManagementAPI.Core.Domain.Entities;
    using EventManagementAPI.Core.Domain.Errors;

    public class DeleteEventCommandHandler : ICommandHandler<DeleteEventCommand, Result<string>>
    {
        private readonly IRepository<Event> eventRepository;
        private IUserService userService;
        private readonly IUnitOfWork unitOfWork;

        public DeleteEventCommandHandler(IRepository<Event> eventRepository, IUserService userService, IUnitOfWork unitOfWork)
        {
            this.eventRepository = eventRepository;
            this.userService = userService;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<string>> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
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

                var user = await this.userService.GetUserDetailsByIdAsync(request.UserId);
                if (user == null)
                {
                    return Result<string>.Failure(DomainErrors.Auth.UserNotFound(request.UserId));
                }

                var eventDetails = await this.eventRepository.GetByIdAsync(request.Id);
                if (eventDetails == null)
                {
                    return Result<string>.Failure(DomainErrors.Event.NotFound(request.Id));
                }

                if (eventDetails.CreatedBy != request.UserId)
                {
                    return Result<string>.Failure(DomainErrors.Event.NotCreatedByUser(request.UserId));
                }

                await this.eventRepository.DeleteAsync(request.Id);

                await this.unitOfWork.SaveChangesAsync(cancellationToken);
                await this.unitOfWork.CommitAsync();

                return Result<string>.Success("Removed the event successfully");
            }
            catch (Exception)
            {
                await this.unitOfWork.RollbackAsync();
                return Result<string>.Failure(DomainErrors.Transaction.TransactionFailed());
            }
        }
    }
}
