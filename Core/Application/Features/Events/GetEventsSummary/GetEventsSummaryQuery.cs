// <copyright file="GetEventsSummaryQuery.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Features.Events.GetEventsSummary
{
    using System.Security.Claims;
    using EventManagementAPI.Core.Application.Contracts.Messaging.Query;
    using EventManagementAPI.Core.Application.DTO;
    using EventManagementAPI.Core.Application.Response;

    public class GetEventsSummaryQuery : IQuery<Result<EventSummaryDTO>>
    {
        public ClaimsPrincipal? User { get; set; }
    }
}
