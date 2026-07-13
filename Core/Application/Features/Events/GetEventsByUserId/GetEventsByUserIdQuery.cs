// <copyright file="GetEventsByUserIdQuery.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Features.Events.GetEventsByUserId
{
    using System.Security.Claims;
    using EventManagementAPI.Core.Application.Contracts.Messaging.Query;
    using EventManagementAPI.Core.Application.DTO;
    using EventManagementAPI.Core.Application.Response;

    public class GetEventsByUserIdQuery : IQuery<Result<List<EventDTO>>>
    {
        public ClaimsPrincipal? User { get; set; }

        public Guid UserId { get; set; }
    }
}
