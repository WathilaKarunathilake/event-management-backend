// <copyright file="NotificationMessageDTO.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.DTO
{
    using EventManagementAPI.Core.Domain.Enums;

    public class NotificationMessageDTO
    {
        public string? Subject { get; set; }

        public string? Content { get; set; }

        public List<string> Recipients { get; set; } = new ();

        public NotificationType Type { get; set; }
    }
}
