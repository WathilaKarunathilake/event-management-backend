// <copyright file="GetEventsByUserIdQueryHandler.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Features.Events.GetEventsByUserId
{
    using AutoMapper;
    using EventManagementAPI.Core.Application.Contracts.Messaging.Query;
    using EventManagementAPI.Core.Application.Contracts.Persistence;
    using EventManagementAPI.Core.Application.DTO;
    using EventManagementAPI.Core.Application.Extensions;
    using EventManagementAPI.Core.Application.Response;
    using EventManagementAPI.Core.Domain.Entities;
    using EventManagementAPI.Core.Domain.Errors;

    public class GetEventsByUserIdQueryHandler : IQueryHandler<GetEventsByUserIdQuery, Result<List<EventDTO>>>
    {
        private readonly IRepository<Event> eventRepository;
        private readonly IMapper mapper;

        public GetEventsByUserIdQueryHandler(IRepository<Event> eventRepository, IMapper mapper)
        {
            this.eventRepository = eventRepository;
            this.mapper = mapper;
        }

        public async Task<Result<List<EventDTO>>> Handle(GetEventsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var userIdString = request.User!.GetUserId();
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            {
                return Result<List<EventDTO>>.Failure(DomainErrors.Auth.NotAuthenticated());
            }

            request.UserId = userId;

            var events = await this.eventRepository.FindAllAsync(x => x.CreatedBy == request.UserId);
            var eventDtos = this.mapper.Map<List<EventDTO>>(events);
            return Result<List<EventDTO>>.Success(eventDtos);
        }
    }
}
