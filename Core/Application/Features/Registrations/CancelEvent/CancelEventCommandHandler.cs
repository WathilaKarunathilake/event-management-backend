// <copyright file="CancelEventCommandHandler.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Features.Registrations.CancelEvent
{
    using AutoMapper;
    using EventManagementAPI.Core.Application.Contracts.Messaging.Commands;
    using EventManagementAPI.Core.Application.Contracts.Persistence;
    using EventManagementAPI.Core.Application.Response;
    using EventManagementAPI.Core.Domain.Entities;
    using EventManagementAPI.Core.Domain.Errors;

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
            var registration = await this.registrationRepository.FindFirstOrDefaultAsync(x => x.EventId == request.EventId);

            if (registration == null)
            {
                return Result<string>.Failure(DomainErrors.Registration.NotFoundForEvent(request.EventId));
            }

            if (registration.RegisterType == Domain.Enums.RegisterType.CANCELED)
            {
                return Result<string>.Failure(DomainErrors.Registration.AlreadyCanceled());
            }

            registration.RegisterType = Domain.Enums.RegisterType.CANCELED;

            await this.registrationRepository.UpdateAsync(registration);
            await this.registrationRepository.SaveAsync();

            return Result<string>.Success("Event canceled successfully!");
        }
    }
}
