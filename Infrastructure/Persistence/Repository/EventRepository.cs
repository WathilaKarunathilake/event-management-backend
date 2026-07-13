// <copyright file="EventRepository.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Infrastructure.Persistence.Repository
{
    using System.Threading.Tasks;
    using EventManagementAPI.Core.Application.Contracts.Persistence;
    using EventManagementAPI.Core.Application.DTO;
    using EventManagementAPI.Core.Domain.Entities;
    using EventManagementAPI.Infrastructure.Persistence.Context;
    using Microsoft.EntityFrameworkCore;

    public class EventRepository : Repository<Event>, IEventRepository
    {
        private readonly AppDbContext context;

        public EventRepository(AppDbContext context)
            : base(context)
        {
            this.context = context;
        }

        public async Task<PageResultDTO<Event>> GetPagedEventsAsync(
            int? pageNumber,
            int? pageSize,
            string? searchTerm,
            string? sortBy)
        {
            var query = this.context.Events.AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(e => e.Title!.ToLower().Contains(searchTerm.ToLower()));
            }

            query = sortBy?.ToLower() switch
            {
                "name-desc" => query.OrderByDescending(e => e.Title),
                "name-asc" => query.OrderBy(e => e.Title),
                "date-asc" => query.OrderBy(e => e.StartDateTime),
                "date-desc" => query.OrderByDescending(e => e.StartDateTime),
                "upc-only" => query.Where(e => e.StartDateTime > DateTime.UtcNow)
                              .OrderBy(e => e.StartDateTime),
                "exp-only" => query.Where(e => e.StartDateTime < DateTime.UtcNow)
                                      .OrderByDescending(e => e.StartDateTime),
                _ => query.OrderBy(e => e.Id)
            };

            var totalCount = await query.CountAsync();
            if (pageNumber.HasValue && pageSize.HasValue && pageNumber > 0 && pageSize > 0)
            {
                query = query
                    .Skip((pageNumber.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value);
            }

            var events = await query.ToListAsync();

            return new PageResultDTO<Event>
            {
                TotalCount = totalCount,
                Items = events,
            };
        }
    }
}
