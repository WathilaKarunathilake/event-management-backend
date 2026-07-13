// <copyright file="NotificationSendCommandHandler.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Features.Notification.NotificationSendCommand
{
    using EventManagementAPI.Core.Application.Contracts.Messaging.Commands;
    using EventManagementAPI.Core.Application.Contracts.Notification;
    using EventManagementAPI.Core.Application.Response;
    using EventManagementAPI.Core.Domain.Errors;

    public class NotificationSendCommandHandler : ICommandHandler<NotificationSendCommand, Result<bool>>
    {
        private readonly INotificationSenderService notification;

        public NotificationSendCommandHandler(INotificationSenderService notification)
        {
            this.notification = notification;
        }

        public async Task<Result<bool>> Handle(NotificationSendCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await this.notification.SendAsync(request.NotificationMessage!, cancellationToken);
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(DomainErrors.Custom.Failure(ex.Message));
            }
        }
    }
}
