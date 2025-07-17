// <copyright file="GetRegistrationsByEventIdQueryHandler.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Features.Registrations.GetRegistrations
{
    using EventManagementAPI.Core.Application.Contracts.Identity;
    using EventManagementAPI.Core.Application.Contracts.Messaging.Query;
    using EventManagementAPI.Core.Application.Contracts.Persistence;
    using EventManagementAPI.Core.Application.DTO;
    using EventManagementAPI.Core.Application.Response;
    using EventManagementAPI.Core.Domain.Entities;
    using EventManagementAPI.Core.Domain.Errors;

    public class GetRegistrationsByEventIdQueryHandler : IQueryHandler<GetRegistrationsByEventIdQuery, Result<List<UserDataDTO>>>
    {
        private readonly IRepository<Registration> registrationRepository;
        private readonly IRepository<Event> eventRepository;
        private readonly IUserService userService;

        public GetRegistrationsByEventIdQueryHandler(IRepository<Registration> registrationRepository, IRepository<Event> eventRepository, IUserService userService)
        {
            this.registrationRepository = registrationRepository;
            this.eventRepository = eventRepository;
            this.userService = userService;
        }

        public async Task<Result<List<UserDataDTO>>> Handle(GetRegistrationsByEventIdQuery request, CancellationToken cancellationToken)
        {
            var eventDetails = await this.eventRepository.GetByIdAsync(request.EventId);
            if (eventDetails == null)
            {
                return Result<List<UserDataDTO>>.Failure(DomainErrors.Event.NotFound(request.EventId));
            }

            var registrations = await this.registrationRepository.FindAllAsync(r => r.EventId == request.EventId);
            var userIds = registrations.Select(r => r.UserId).Distinct().ToList();

            var users = await this.userService.GetUsersByIdsAsync(userIds);
            return Result<List<UserDataDTO>>.Success(users);
        }
    }
}
