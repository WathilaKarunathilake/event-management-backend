// <copyright file="GetEventsSummaryQueryHandler.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Features.Events.GetEventsSummary
{
    using System.Threading;
    using System.Threading.Tasks;
    using EventManagementAPI.Core.Application.Contracts.Messaging.Query;
    using EventManagementAPI.Core.Application.Contracts.Persistence;
    using EventManagementAPI.Core.Application.DTO;
    using EventManagementAPI.Core.Application.Extensions;
    using EventManagementAPI.Core.Application.Response;
    using EventManagementAPI.Core.Domain.Entities;

    public class GetEventsSummaryQueryHandler : IQueryHandler<GetEventsSummaryQuery, Result<EventSummaryDTO>>
    {
        private IRepository<Event> eventRepository;
        private IRepository<Registration> registrationRepository;

        public GetEventsSummaryQueryHandler(IRepository<Event> eventRepository, IRepository<Registration> registrationRepository)
        {
            this.eventRepository = eventRepository;
            this.registrationRepository = registrationRepository;
        }

        public async Task<Result<EventSummaryDTO>> Handle(GetEventsSummaryQuery request, CancellationToken cancellationToken)
        {
            var userIdString = request.User!.GetUserId();
            var events = await this.eventRepository.FindAllAsync(x => x.CreatedBy.ToString() == userIdString);

            var totalEvents = events.Count;
            var totalActiveEvents = events.Count(e => e.TotalRegistrations < e.Capacity);
            var totalFilledEvents = events.Count(e => e.TotalRegistrations >= e.Capacity);
            var totalUpcomingEvents = events.Count(e =>
    DateTime.SpecifyKind(e.StartDateTime, DateTimeKind.Utc).ToLocalTime() > DateTime.Now);

            var eventIds = events.Select(e => e.Id).ToList();
            var registrations = await this.registrationRepository.FindAllAsync(r => eventIds.Contains(r.EventId));
            var totalAttendees = registrations.Count;

            var summary = new EventSummaryDTO
            {
                TotalEvents = totalEvents,
                ActiveEvents = totalActiveEvents,
                FilledEvents = totalFilledEvents,
                UpcommingEvents = totalUpcomingEvents,
                TotalAttendees = totalAttendees,
            };

            return Result<EventSummaryDTO>.Success(summary);
        }
    }
}
