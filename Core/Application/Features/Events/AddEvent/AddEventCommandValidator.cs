// <copyright file="AddEventCommandValidator.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Features.Events.AddEvent
{
    using EventManagementAPI.Core.Domain.Errors;
    using FluentValidation;

    public class AddEventCommandValidator : AbstractValidator<AddEventCommand>
    {
        public AddEventCommandValidator()
        {
            this.RuleFor(x => x.Title)
               .NotEmpty().WithMessage(DomainErrors.Event.TitleIsRequired().Message)
               .MaximumLength(100).WithMessage(DomainErrors.Event.TitleTooLong().Message);

            this.RuleFor(x => x.Description)
                .NotEmpty().WithMessage(DomainErrors.Event.DescriptionIsRequired().Message)
                .MinimumLength(100).WithMessage(DomainErrors.Event.DescriptionTooShort().Message)
                .MaximumLength(1000).WithMessage(DomainErrors.Event.DescriptionTooLong().Message);

            this.RuleFor(x => x.Location)
                .NotEmpty().WithMessage(DomainErrors.Event.LocationIsRequired().Message)
                .MaximumLength(200).WithMessage(DomainErrors.Event.LocationTooLong().Message);

            this.RuleFor(x => x.Capacity)
                .GreaterThan(0).WithMessage(DomainErrors.Event.CapacityMustBePositive().Message)
                .LessThanOrEqualTo(10000).WithMessage(DomainErrors.Event.CapacityTooLarge().Message);

            this.RuleFor(x => x.StartDateTime)
                .GreaterThan(DateTime.Now).WithMessage(DomainErrors.Event.StartTimeInPast().Message);

            this.RuleFor(x => x.EndDateTime)
                .GreaterThan(x => x.StartDateTime).WithMessage(DomainErrors.Event.EndTimeBeforeStart().Message);

            this.RuleFor(x => x.CreatedBy)
                .NotEmpty().WithMessage(DomainErrors.Event.CreatorIdRequired().Message);
        }
    }
}
