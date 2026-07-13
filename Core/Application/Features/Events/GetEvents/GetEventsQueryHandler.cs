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

    public class GetEventsQueryHandler : IQueryHandler<GetEventsQuery, Result<PageResultDTO<EventDTO>>>
    {
        private readonly IMapper mapper;
        private readonly IEventRepository eventRepository;

        public GetEventsQueryHandler(IMapper mapper, IEventRepository eventRepository)
        {
            this.mapper = mapper;
            this.eventRepository = eventRepository;
        }

        public async Task<Result<PageResultDTO<EventDTO>>> Handle(GetEventsQuery request, CancellationToken cancellationToken)
        {
            var events = await this.eventRepository.GetPagedEventsAsync(request.Page, request.PageSize, request.SearchTerm, request.SortBy);
            var eventDtos = this.mapper.Map<List<EventDTO>>(events.Items);

            var pageResult = new PageResultDTO<EventDTO>
            {
                Items = eventDtos,
                TotalCount = events.TotalCount,
            };

            return Result<PageResultDTO<EventDTO>>.Success(pageResult);
        }
    }
}
