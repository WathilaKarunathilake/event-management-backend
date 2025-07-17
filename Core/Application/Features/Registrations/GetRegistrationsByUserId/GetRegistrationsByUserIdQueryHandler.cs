// <copyright file="GetRegistrationsByUserIdQueryHandler.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Features.Registrations.GetRegistrationsById
{
    using AutoMapper;
    using EventManagementAPI.Core.Application.Contracts.Identity;
    using EventManagementAPI.Core.Application.Contracts.Messaging.Query;
    using EventManagementAPI.Core.Application.Contracts.Persistence;
    using EventManagementAPI.Core.Application.DTO;
    using EventManagementAPI.Core.Application.Response;
    using EventManagementAPI.Core.Domain.Entities;
    using EventManagementAPI.Core.Domain.Errors;

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
            var registrations = await this.registrationRepository.FindAllAsync(r => r.EventId == request.UserId);
            var eventIds = registrations.Select(r => r.EventId).Distinct().ToList();

            var events = await this.eventRepository.FindAllAsync(x => eventIds.Contains(x.Id));

            var eventDtos = this.mapper.Map<List<EventDTO>>(events);

            return Result<List<EventDTO>>.Success(eventDtos);
        }
    }
}
