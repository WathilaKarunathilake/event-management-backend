// <copyright file="DeleteEventCommandHandler.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Features.Events.DeleteEvent
{
    using EventManagementAPI.Core.Application.Contracts.Messaging.Commands;
    using EventManagementAPI.Core.Application.Contracts.Persistence;
    using EventManagementAPI.Core.Application.Response;
    using EventManagementAPI.Core.Domain.Entities;
    using EventManagementAPI.Core.Domain.Errors;

    public class DeleteEventCommandHandler : ICommandHandler<DeleteEventCommand, Result<string>>
    {
        private readonly IRepository<Event> eventRepository;

        public DeleteEventCommandHandler(IRepository<Event> eventRepository)
        {
            this.eventRepository = eventRepository;
        }

        public async Task<Result<string>> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
        {
            var eventDetails = await this.eventRepository.GetByIdAsync(request.Id);
            if (eventDetails == null)
            {
                return Result<string>.Failure(DomainErrors.Event.NotFound(request.Id));
            }

            await this.eventRepository.DeleteAsync(request.Id);
            return Result<string>.Success("Removed the event successfully");
        }
    }
}
