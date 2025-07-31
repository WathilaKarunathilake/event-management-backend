// <copyright file="NotificationMessage.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Infrastructure.Notification.Models
{
    using EventManagementAPI.Infrastructure.Notification.Enums;

    public class NotificationMessage
    {
        public string? Subject { get; set; }

        public string? Content { get; set; }

        public List<string> Recipients { get; set; } = new ();

        public NotificationType Type { get; set; }
    }
}
