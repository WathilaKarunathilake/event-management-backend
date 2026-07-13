// <copyright file="GetEventByIdQueryHandler.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Features.Events.GetEventById
{
    using AutoMapper;
    using EventManagementAPI.Core.Application.Contracts.Messaging.Query;
    using EventManagementAPI.Core.Application.Contracts.Persistence;
    using EventManagementAPI.Core.Application.DTO;
    using EventManagementAPI.Core.Application.Response;
    using EventManagementAPI.Core.Domain.Entities;
    using EventManagementAPI.Core.Domain.Errors;

    public class GetEventByIdQueryHandler : IQueryHandler<GetEventByIdQuery, Result<EventDTO>>
    {
        private readonly IMapper mapper;
        private readonly IRepository<Event> eventRepository;

        public GetEventByIdQueryHandler(IMapper mapper, IRepository<Event> eventRepository)
        {
            this.mapper = mapper;
            this.eventRepository = eventRepository;
        }

        public async Task<Result<EventDTO>> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
        {
            var eventDetails = await this.eventRepository.GetByIdAsync(request.Id);
            if (eventDetails == null)
            {
                return Result<EventDTO>.Failure(DomainErrors.Event.NotFound(request.Id));
            }

            return Result<EventDTO>.Success(this.mapper.Map<EventDTO>(eventDetails));
        }
    }
}
