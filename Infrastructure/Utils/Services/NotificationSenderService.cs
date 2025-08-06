// <copyright file="NotificationSenderService.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Infrastructure.Utils.Services
{
    using System.Threading;
    using System.Threading.Tasks;
    using EventManagementAPI.Core.Application.Contracts.Notification;
    using EventManagementAPI.Core.Application.DTO;
    using WathilaKarunathilake.Notification.Contracts;
    using WathilaKarunathilake.Notification.Models;

    public class NotificationSenderService : INotificationSenderService
    {
        private readonly INotificationService notificationService;

        public NotificationSenderService(INotificationService notificationService)
        {
            this.notificationService = notificationService;
        }

        public async Task SendAsync(NotificationMessageDTO message, CancellationToken cancellationToken = default)
        {
            var notificaitionMsg = new NotificationMessage
            {
                Content = message.Content,
                Recipients = message.Recipients,
                Subject = message.Subject,
                Type = (WathilaKarunathilake.Notification.Enums.NotificationType)message.Type,
            };

            await this.notificationService.SendAsync(notificaitionMsg, cancellationToken);
        }
    }
}
