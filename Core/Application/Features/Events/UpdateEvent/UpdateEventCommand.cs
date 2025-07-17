// <copyright file="UpdateEventCommand.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Features.Events.UpdateEvent
{
    using EventManagementAPI.Core.Application.Contracts.Messaging.Commands;
    using EventManagementAPI.Core.Application.Response;

    public class UpdateEventCommand : ICommand<Result<string>>
    {
        public Guid Id { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? Location { get; set; }

        public DateTime StartDateTime { get; set; }

        public int Capacity { get; set; }

        public DateTime EndDateTime { get; set; }
    }
}
