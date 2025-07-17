// <copyright file="RegistrationCommandValidator.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Features.Auth.Register
{
    using EventManagementAPI.Core.Domain.Errors;
    using FluentValidation;

    public class RegistrationCommandValidator : AbstractValidator<RegistrationCommand>
    {
        public RegistrationCommandValidator()
        {
            this.RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage(DomainErrors.Auth.EmailIsRequired().Message)
                .EmailAddress()
                .WithMessage(DomainErrors.Auth.EmailInvalidFormat().Message);

            this.RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage(DomainErrors.Auth.PasswordIsRequired().Message)
                .MinimumLength(6)
                .WithMessage(DomainErrors.Auth.PasswordTooShort().Message);

            this.RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .WithMessage(DomainErrors.Auth.PhoneNumberIsRequired().Message)
                .Matches(@"^\+?\d{10,15}$")
                .WithMessage(DomainErrors.Auth.PhoneNumberInvalid().Message);

            this.RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(DomainErrors.Auth.NameIsRequired().Message)
                .MaximumLength(100)
                .WithMessage(DomainErrors.Auth.NameTooLong().Message);

            this.RuleFor(x => x.Role)
                .IsInEnum()
                .WithMessage(DomainErrors.Auth.InvalidRole().Message);
        }
    }
}
