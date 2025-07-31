// <copyright file="GetRegistrationsByUserIdQueryHandler.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Features.Registrations.GetRegistrationsById
{
    using EventManagementAPI.Core.Application.Contracts.Messaging.Query;
    using EventManagementAPI.Core.Application.Contracts.Persistence;
    using EventManagementAPI.Core.Application.DTO;
    using EventManagementAPI.Core.Application.Extensions;
    using EventManagementAPI.Core.Application.Response;
    using EventManagementAPI.Core.Domain.Entities;
    using EventManagementAPI.Core.Domain.Errors;

    public class GetRegistrationsByUserIdQueryHandler : IQueryHandler<GetRegistrationsByUserIdQuery, Result<List<RegisteredEventsDTO>>>
    {
        private readonly IRepository<Registration> registrationRepository;
        private readonly IRepository<Event> eventRepository;

        public GetRegistrationsByUserIdQueryHandler(IRepository<Registration> registrationRepository, IRepository<Event> eventRepository)
        {
            this.registrationRepository = registrationRepository;
            this.eventRepository = eventRepository;
        }

        public async Task<Result<List<RegisteredEventsDTO>>> Handle(GetRegistrationsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var userIdString = request.User!.GetUserId();
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            {
                return Result<List<RegisteredEventsDTO>>.Failure(DomainErrors.Auth.NotAuthenticated());
            }

            request.UserId = userId;

            var registrations = await this.registrationRepository.FindAllAsync(r => r.UserId == request.UserId);

            var eventIds = registrations.Select(r => r.EventId).Distinct().ToList();
            var events = await this.eventRepository.FindAllAsync(x => eventIds.Contains(x.Id));
            var result = registrations
                .Join(
                    events,
                    reg => reg.EventId,
                    ev => ev.Id,
                    (reg, ev) => new RegisteredEventsDTO
                        {
                            Title = ev.Title ?? string.Empty,
                            Description = ev.Description,
                            Location = ev.Location,
                            StartDateTime = ev.StartDateTime,
                            EndDateTime = ev.EndDateTime,
                            EventType = ev.EventType,
                            Capacity = ev.Capacity,
                            RegisterType = reg.RegisterType,
                            CreatedBy = ev.CreatedBy,
                            ImageUrl = ev.ImageUrl,
                            CreatorName = ev.CreatorName,
                            Id = ev.Id,
                        })
                .ToList();

            return Result<List<RegisteredEventsDTO>>.Success(result);
        }
    }
}
