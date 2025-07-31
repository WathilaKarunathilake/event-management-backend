// <copyright file="INotificationChannel.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Infrastructure.Notification.Contracts
{
    using EventManagementAPI.Infrastructure.Notification.Enums;
    using EventManagementAPI.Infrastructure.Notification.Models;

    public interface INotificationChannel
    {
        NotificationType Type { get; }

        Task SendAsync(NotificationMessage message, CancellationToken cancellationToken = default);
    }
}
