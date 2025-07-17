// <copyright file="Registration.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Domain.Entities
{
    using EventManagementAPI.Core.Domain.Enums;

    public class Registration
    {
        public Guid Id { get; set; }

        public Guid EventId { get; set; }

        public Guid UserId { get; set; }

        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;

        public RegisterType RegisterType { get; set; } = RegisterType.REGISTERED;
    }
}
