// <copyright file="NotificationManager.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Infrastructure.Notification
{
    using EventManagementAPI.Infrastructure.Notification.Contracts;
    using EventManagementAPI.Infrastructure.Notification.Models;

    public class NotificationManager : INotificationService
    {
        private readonly IEnumerable<INotificationChannel> channels;

        public NotificationManager(IEnumerable<INotificationChannel> channels)
        {
            this.channels = channels;
        }

        public async Task SendAsync(NotificationMessage message, CancellationToken cancellationToken = default)
        {
            var channel = this.channels.FirstOrDefault(c => c.Type == message.Type);

            if (channel is null)
            {
                throw new InvalidOperationException($"Notification channel not found for type: {message.Type}");
            }

            await channel.SendAsync(message, cancellationToken);
        }
    }
}
