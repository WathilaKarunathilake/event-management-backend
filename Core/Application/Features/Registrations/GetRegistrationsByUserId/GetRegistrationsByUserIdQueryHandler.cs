// <copyright file="GetRegistrationsByIdQueryHandler.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Features.Registrations.GetRegistrationsById
{
    using AutoMapper;
    using EventManagementAPI.Core.Application.Contracts.Messaging.Query;
    using EventManagementAPI.Core.Application.Contracts.Persistence;
    using EventManagementAPI.Core.Application.DTO;
    using EventManagementAPI.Core.Application.Response;
    using EventManagementAPI.Core.Domain.Entities;

    public class GetRegistrationsByUserIdQueryHandler : IQueryHandler<GetRegistrationsByUserIdQuery, Result<List<EventDTO>>>
    {
        private readonly IRepository<Registration> registrationRepository;
        private readonly IRepository<Event> eventRepository;
        private readonly IMapper mapper;

        public GetRegistrationsByUserIdQueryHandler(IRepository<Registration> registrationRepository, IRepository<Event> eventRepository, IMapper mapper)
        {
            this.registrationRepository = registrationRepository;
            this.eventRepository = eventRepository;
            this.mapper = mapper;
        }

        public async Task<Result<List<EventDTO>>> Handle(GetRegistrationsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var registrations = await registrationRepository.FindAllAsync(x => x.UserId == request.UserId);
            var eventIds = registrations.Select(r => r.EventId).Distinct().ToList();

            var events = await eventRepository.FindAllAsync(e => eventIds.Contains(e.Id));

            var eventDtos = mapper.Map<List<EventDTO>>(events);
            return Result<List<EventDTO>>.Success(eventDtos);
        }
    }
}
