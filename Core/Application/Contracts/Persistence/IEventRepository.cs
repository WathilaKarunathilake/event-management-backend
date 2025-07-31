// <copyright file="IEventRepository.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Contracts.Persistence
{
    using EventManagementAPI.Core.Application.DTO;
    using EventManagementAPI.Core.Domain.Entities;

    public interface IEventRepository : IRepository<Event>
    {
        Task<PageResultDTO<Event>> GetPagedEventsAsync(
            int? pageNumber,
            int? pageSize,
            string? searchTerm,
            string? sortBy);
    }
}
