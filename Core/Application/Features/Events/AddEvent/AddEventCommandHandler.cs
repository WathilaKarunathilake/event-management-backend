// <copyright file="AddEventCommandHandler.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Features.Events.AddEvent
{
    using AutoMapper;
    using EventManagementAPI.Core.Application.Contracts.Messaging.Commands;
    using EventManagementAPI.Core.Application.Contracts.Persistence;
    using EventManagementAPI.Core.Application.Response;
    using EventManagementAPI.Core.Domain.Entities;

    public class AddEventCommandHandler : ICommandHandler<AddEventCommand, Result<string>>
    {
        private readonly IRepository<Event> eventRepository;
        private readonly IMapper mapper;

        public AddEventCommandHandler(IRepository<Event> eventRepository, IMapper mapper)
        {
            this.eventRepository = eventRepository;
            this.mapper = mapper;
        }

        public async Task<Result<string>> Handle(AddEventCommand request, CancellationToken cancellationToken)
        {
            var eventRequest = this.mapper.Map<Event>(request);
            await this.eventRepository.AddAsync(eventRequest);
            await this.eventRepository.SaveAsync();

            return Result<string>.Success("Event added successfully !");
        }
    }
}
