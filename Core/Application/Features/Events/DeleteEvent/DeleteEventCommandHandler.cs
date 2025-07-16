using EventManagementAPI.Core.Application.Contracts.Messaging.Commands;
using EventManagementAPI.Core.Application.Contracts.Persistence;
using EventManagementAPI.Core.Application.DTO;
using EventManagementAPI.Core.Application.Response;
using EventManagementAPI.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementAPI.Core.Application.Features.Events.DeleteEvent
{
    public class DeleteEventCommandHandler : ICommandHandler<DeleteEventCommand, Result<string>>
    {
        private readonly IRepository<Event> eventRepository;

        public DeleteEventCommandHandler(IRepository<Event> eventRepository)
        {
            this.eventRepository = eventRepository;
        }

        public async Task<Result<string>> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
        {
            var eventDetails = await eventRepository.GetByIdAsync(request.Id);
            if (eventDetails == null)
            {
                return Result<string>.Failure("Unable to find the event");
            }

            await eventRepository.DeleteAsync(request.Id);
            return Result<string>.Success("Removed the event successfully");
        }
    }
}
