// <copyright file="RegisterEventCommandValidator.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Features.Registrations.RegisterEvent
{
    using EventManagementAPI.Core.Domain.Errors;
    using FluentValidation;

    public class RegisterEventCommandValidator : AbstractValidator<RegisterEventCommand>
    {
        public RegisterEventCommandValidator()
        {
            this.RuleFor(x => x.EventId)
                .NotEmpty().WithMessage(DomainErrors.Registration.EventIdRequired().Message);

            this.RuleFor(x => x.UserId)
                .NotEmpty().WithMessage(DomainErrors.Registration.UserIdRequired().Message);
        }
    }
}
