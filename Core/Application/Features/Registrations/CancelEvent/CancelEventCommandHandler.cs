// <copyright file="CancelEventCommandHandler.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
using AutoMapper;
using EventManagementAPI.Core.Application.Contracts.Messaging.Commands;
using EventManagementAPI.Core.Application.Contracts.Persistence;
using EventManagementAPI.Core.Application.Features.Registrations.RegisterEvent;
using EventManagementAPI.Core.Application.Response;
using EventManagementAPI.Core.Domain.Entities;

namespace EventManagementAPI.Core.Application.Features.Registrations.CancelEvent
{
    public class CancelEventCommandHandler : ICommandHandler<CancelEventCommand, Result<string>>
    {

        private readonly IRepository<Registration> registrationRepository;
        private readonly IMapper mapper;

        public CancelEventCommandHandler(IRepository<Registration> registrationRepository, IMapper mapper)
        {
            this.registrationRepository = registrationRepository;
            this.mapper = mapper;
        }

        public async Task<Result<string>> Handle(CancelEventCommand request, CancellationToken cancellationToken)
        {
            var eventDetails = await registrationRepository.GetAllAsync();

        }
    }
}
