// <copyright file="RegisterEventCommandHandler.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
using AutoMapper;
using EventManagementAPI.Core.Application.Contracts.Messaging.Commands;
using EventManagementAPI.Core.Application.Contracts.Persistence;
using EventManagementAPI.Core.Application.Response;
using EventManagementAPI.Core.Domain.Entities;

namespace EventManagementAPI.Core.Application.Features.Registrations.RegisterEvent
{
    public class RegisterEventCommandHandler : ICommandHandler<RegisterEventCommand, Result<string>>
    {
        private readonly IRepository<Event> eventRepository;
        private readonly IRepository<Registration> registrationRepository;
        private readonly IMapper mapper;

        public RegisterEventCommandHandler(IRepository<Event> eventRepository, IRepository<Registration> registrationRepository, IMapper mapper)
        {
            this.eventRepository = eventRepository;
            this.registrationRepository = registrationRepository;
            this.mapper = mapper;
        }

        public async Task<Result<string>> Handle(RegisterEventCommand request, CancellationToken cancellationToken)
        {
            var registration = mapper.Map<Registration>(request);
            await registrationRepository.AddAsync(registration);
            return Result<string>.Success("Registered successfully.");
        }
    }
}
