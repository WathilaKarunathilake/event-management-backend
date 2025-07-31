// <copyright file="InAppNotificationSendingService.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Infrastructure.Notification.Channels
{
    using System.Threading;
    using EventManagementAPI.Infrastructure.Notification.Contracts;
    using EventManagementAPI.Infrastructure.Notification.Enums;
    using EventManagementAPI.Infrastructure.Notification.Hubs;
    using EventManagementAPI.Infrastructure.Notification.Models;
    using Microsoft.AspNetCore.SignalR;

    public class InAppNotificationSendingService : INotificationChannel
    {
        private readonly IHubContext<NotificationHub> hubContext;

        public InAppNotificationSendingService(IHubContext<NotificationHub> hubContext)
        {
            this.hubContext = hubContext;
        }

        public NotificationType Type => NotificationType.Inapp;

        public async Task SendAsync(NotificationMessage message, CancellationToken cancellationToken = default)
        {
            foreach (var recipient in message.Recipients)
            {
                await this.hubContext.Clients.Group(recipient).SendAsync("ReceiveMessage", message, cancellationToken);
            }
        }
    }
}
