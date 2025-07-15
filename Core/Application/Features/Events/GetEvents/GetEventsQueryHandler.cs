// <copyright file="GetEventsQueryHandler.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Features.Events.GetEvents
{
    using AutoMapper;
    using EventManagementAPI.Core.Application.Contracts.Messaging.Query;
    using EventManagementAPI.Core.Application.Contracts.Persistence;
    using EventManagementAPI.Core.Application.DTO;
    using EventManagementAPI.Core.Application.Response;
    using EventManagementAPI.Core.Domain.Entities;

    public class GetEventsQueryHandler : IQueryHandler<GetEventsQuery, Result<List<EventDTO>>>
    {
        private readonly IMapper mapper;
        private readonly IRepository<Event> eventRepository;

        public GetEventsQueryHandler(IMapper mapper, IRepository<Event> eventRepository)
        {
            this.mapper = mapper;
            this.eventRepository = eventRepository;
        }

        public async Task<Result<List<EventDTO>>> Handle(GetEventsQuery request, CancellationToken cancellationToken)
        {
            var events = await this.eventRepository.GetAllAsync();
            return Result<List<EventDTO>>.Success(this.mapper.Map<List<EventDTO>>(events));
        }
    }
}
