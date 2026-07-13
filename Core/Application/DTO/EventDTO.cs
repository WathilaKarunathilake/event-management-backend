// <copyright file="EventDTO.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>

namespace EventManagementAPI.Core.Application.DTO
{
    using EventManagementAPI.Core.Domain.Enums;

    public class EventDTO
    {
        public Guid Id { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? Location { get; set; }

        public string? CreatorName { get; set; }

        public int Capacity { get; set; }

        public string? ImageUrl { get; set; }

        public int TotalRegistrations { get; set; }

        public EventType EventType { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTime Created { get; set; }
    }
}
