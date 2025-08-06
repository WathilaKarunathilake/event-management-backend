// <copyright file="DeleteEventCommandHandler.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Features.Events.DeleteEvent
{
    using EventManagementAPI.Core.Application.Contracts.Identity;
    using EventManagementAPI.Core.Application.Contracts.Messaging.Commands;
    using EventManagementAPI.Core.Application.Contracts.Persistence;
    using EventManagementAPI.Core.Application.DTO;
    using EventManagementAPI.Core.Application.Extensions;
    using EventManagementAPI.Core.Application.Features.Images.RemoveImage;
    using EventManagementAPI.Core.Application.Features.Notification.NotificationSendCommand;
    using EventManagementAPI.Core.Application.Response;
    using EventManagementAPI.Core.Domain.Entities;
    using EventManagementAPI.Core.Domain.Enums;
    using EventManagementAPI.Core.Domain.Errors;
    using MediatR;

    public class DeleteEventCommandHandler : ICommandHandler<DeleteEventCommand, Result<string>>
    {
        private readonly IRepository<Event> eventRepository;
        private readonly IRepository<Registration> registrationRepository;
        private readonly IUserService userService;
        private readonly IUnitOfWork unitOfWork;
        private readonly ISender sender;

        public DeleteEventCommandHandler(IRepository<Event> eventRepository, IRepository<Registration> registrationRepository, IUserService userService, IUnitOfWork unitOfWork, ISender sender)
        {
            this.eventRepository = eventRepository;
            this.registrationRepository = registrationRepository;
            this.userService = userService;
            this.unitOfWork = unitOfWork;
            this.sender = sender;
        }

        public async Task<Result<string>> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
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

                bool isUpcoming = DateTime.SpecifyKind(eventDetails.StartDateTime, DateTimeKind.Utc).ToLocalTime() > DateTime.Now;

                if (isUpcoming)
                {
                    var registrations = await this.registrationRepository.FindAllAsync(r =>
                        r.EventId == request.Id && r.RegisterType == RegisterType.REGISTERED);

                    if (registrations.Any())
                    {
                        return Result<string>.Failure(DomainErrors.Event.CannotDeleteWithActiveRegistrations());
                    }
                }

                if (eventDetails.CreatedBy != request.UserId)
                {
                    return Result<string>.Failure(DomainErrors.Event.NotCreatedByUser(request.UserId));
                }

                // Only then delete the image
                await this.sender.Send(new RemoveImageCommand { ImageName = request.Id.ToString() });
                await this.eventRepository.DeleteAsync(request.Id);

                var inApp = new NotificationMessageDTO
                {
                    Type = NotificationType.Inapp,
                    Subject = "Event removed !",
                    Content = $"Event {eventDetails.Title} just removed",
                    Recipients = new List<string> { UserRole.PUBLICUSER.ToString() },
                };
                await this.sender.Send(new NotificationSendCommand { NotificationMessage = inApp });

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
