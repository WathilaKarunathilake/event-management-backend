using AutoMapper;
using EventManagementAPI.Core.Application.Contracts.Messaging.Query;
using EventManagementAPI.Core.Application.Contracts.Persistence;
using EventManagementAPI.Core.Application.DTO;
using EventManagementAPI.Core.Application.Response;
using EventManagementAPI.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementAPI.Core.Application.Features.Events.GetEventById
{
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
            var eventDetails = await eventRepository.GetByIdAsync(request.Id);
            if (eventDetails == null)
            {
                return Result<EventDTO>.Failure("Unable to find the event");
            }
            return Result<EventDTO>.Success(mapper.Map<EventDTO>(eventDetails));
        }
    }
}
