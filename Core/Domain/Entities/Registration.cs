// <copyright file="Registration.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
using EventManagementAPI.Core.Domain.Enums;

namespace EventManagementAPI.Core.Domain.Entities
{
    public class Registration
    {
        public Guid Id { get; set; }

        public Guid EventId { get; set; }
        public Guid UserId { get; set; }
        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
        public RegisterType RegisterType { get; set; } = RegisterType.REGISTERED;
    }
}
