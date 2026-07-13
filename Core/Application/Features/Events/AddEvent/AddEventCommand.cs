// <copyright file="AddEventCommand.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Features.Events.AddEvent
{
    using System.Security.Claims;
    using EventManagementAPI.Core.Application.Contracts.Messaging.Commands;
    using EventManagementAPI.Core.Application.Response;
    using EventManagementAPI.Core.Domain.Enums;

    public class AddEventCommand : ICommand<Result<string>>
    {
        public ClaimsPrincipal? User { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? Location { get; set; }

        public string? ImageUrl { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public EventType EventType { get; set; }

        public int Capacity { get; set; }

        public Guid CreatedBy { get; set; }
    }
}
