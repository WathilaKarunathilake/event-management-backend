// <copyright file="GetEventByIdQuery.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Features.Events.GetEventById
{
    using EventManagementAPI.Core.Application.Contracts.Messaging.Query;
    using EventManagementAPI.Core.Application.DTO;
    using EventManagementAPI.Core.Application.Response;

    public class GetEventByIdQuery : IQuery<Result<EventDTO>>
    {
        public Guid Id { get; set; }
    }
}
