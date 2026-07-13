// <copyright file="GetEventsQuery.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Features.Events.GetEvents
{
    using EventManagementAPI.Core.Application.Contracts.Messaging.Query;
    using EventManagementAPI.Core.Application.DTO;
    using EventManagementAPI.Core.Application.Response;

    public class GetEventsQuery : IQuery<Result<PageResultDTO<EventDTO>>>
    {
        public int? Page { get; set; }

        public int? PageSize { get; set; }

        public string? SortBy { get; set; }

        public string? SearchTerm { get; set; }
    }
}
