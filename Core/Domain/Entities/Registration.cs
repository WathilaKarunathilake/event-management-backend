// <copyright file="Registration.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Domain.Entities
{
    public class Registration
    {
        public Guid Id { get; set; }

        public Guid EventId { get; set; }
        public Guid UserId { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
        public DateTime? CancelledAt { get; set; }


    }
}
