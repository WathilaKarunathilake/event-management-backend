// <copyright file="RegisterEventCommandValidator.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Features.Registrations.RegisterEvent
{
    using EventManagementAPI.Core.Application.Contracts.Persistence;
    using EventManagementAPI.Core.Domain.Entities;
    using EventManagementAPI.Core.Domain.Enums;
    using FluentValidation;

    public class RegisterEventCommandValidator : AbstractValidator<RegisterEventCommand>
    {
        private readonly IRepository<Event> eventRepository;
        private readonly IRepository<Registration> registrationRepository;

        public RegisterEventCommandValidator(
            IRepository<Event> eventRepository,
            IRepository<Registration> registrationRepository)
        {
            this.eventRepository = eventRepository;
            this.registrationRepository = registrationRepository;

            RuleFor(x => x.EventId)
                .NotEmpty().WithMessage("Event ID is required.")
                .MustAsync(EventExists).WithMessage("Event not found.");

            RuleFor(x => x)
                .MustAsync(EventNotFull).WithMessage("Event is full.");

            RuleFor(x => x)
                .MustAsync(WithinCutoff).WithMessage("Registration period has ended.");

            RuleFor(x => x)
                .MustAsync(NotAlreadyRegistered).WithMessage("User has already registered for this event.");
        }

        private async Task<bool> EventExists(Guid eventId, CancellationToken ct)
        {
            var evt = await eventRepository.GetByIdAsync(eventId);
            return evt != null;
        }

        private async Task<bool> EventNotFull(RegisterEventCommand request, CancellationToken ct)
        {
            var evt = await eventRepository.GetByIdAsync(request.EventId);
            if (evt == null) return false;

            var allRegistrations = await registrationRepository.GetAllAsync(); 
            var count = allRegistrations.Count(r => r.EventId == request.EventId && r.RegisterType == RegisterType.REGISTERED);

            return count < evt.Capacity;
        }

        private async Task<bool> WithinCutoff(RegisterEventCommand request, CancellationToken ct)
        {
            var evt = await eventRepository.GetByIdAsync(request.EventId);
            return evt?.StartDateTime == null || DateTime.UtcNow <= evt.StartDateTime;
        }

        private async Task<bool> NotAlreadyRegistered(RegisterEventCommand request, CancellationToken ct)
        {
            var allRegistrations = await registrationRepository.GetAllAsync();
            return !allRegistrations.Any(r =>
                r.EventId == request.EventId &&
                r.UserId == request.UserId &&
                r.RegisterType == RegisterType.REGISTERED);
        }
    }
}
