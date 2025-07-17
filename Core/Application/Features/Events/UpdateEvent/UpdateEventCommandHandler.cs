// <copyright file="UpdateEventCommandHandler.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Features.Events.UpdateEvent
{
    using AutoMapper;
    using EventManagementAPI.Core.Application.Contracts.Messaging.Commands;
    using EventManagementAPI.Core.Application.Contracts.Persistence;
    using EventManagementAPI.Core.Application.Response;
    using EventManagementAPI.Core.Domain.Entities;
    using EventManagementAPI.Core.Domain.Errors;

    public class UpdateEventCommandHandler : ICommandHandler<UpdateEventCommand, Result<string>>
    {
        private readonly IRepository<Event> eventRepository;
        private readonly IMapper mapper;

        public UpdateEventCommandHandler(IRepository<Event> eventRepository, IMapper mapper)
        {
            this.mapper = mapper;
            this.eventRepository = eventRepository;
        }

        public async Task<Result<string>> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
        {
            var eventDetails = await this.eventRepository.GetByIdAsync(request.Id);
            if (eventDetails == null)
            {
                return Result<string>.Failure(DomainErrors.Event.NotFound(request.Id));
            }

            var updateEvent = this.mapper.Map<Event>(request);
            await this.eventRepository.UpdateAsync(updateEvent);
            return Result<string>.Success("Event updated successfully");
        }
    }
}
