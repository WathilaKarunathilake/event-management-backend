// <copyright file="GetRegistrationsQueryHandler.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
using EventManagementAPI.Core.Application.Contracts.Messaging.Query;
using EventManagementAPI.Core.Application.Contracts.Persistence;
using EventManagementAPI.Core.Application.DTO;
using EventManagementAPI.Core.Application.Response;
using EventManagementAPI.Core.Domain.Entities;

namespace EventManagementAPI.Core.Application.Features.Registrations.GetRegistrations
{
    public class GetRegistrationsByEventIdQueryHandler : IQueryHandler<GetRegistrationsByEventIdQuery, Result<List<EventDTO>>>
    {
        private readonly IRepository<Registration> registrationRepository;
        private readonly IRepository<Event> eventRepository;

        public GetRegistrationsByEventIdQueryHandler(IRepository<Registration> registrationRepository, IRepository<Event> eventRepository)
        {
            this.registrationRepository = registrationRepository;
            this.eventRepository = eventRepository;
        }

        public async Task<Result<List<EventDTO>>> Handle(GetRegistrationsByEventIdQuery request, CancellationToken cancellationToken)
        {
            var registrations = await registrationRepository.FindAllAsync(x => x.EventId == request.EventId);
            return Result<List<EventDTO>>.Failure(""); 
        }
    }
}
