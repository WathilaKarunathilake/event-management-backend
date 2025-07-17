// <copyright file="LoginCommandValidator.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Features.Auth.Login
{
    using EventManagementAPI.Core.Domain.Errors;
    using FluentValidation;

    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
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
        }
    }
}
