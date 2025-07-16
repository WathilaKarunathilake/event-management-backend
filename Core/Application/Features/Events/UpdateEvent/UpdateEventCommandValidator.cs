// <copyright file="UpdateEventCommandValidator.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Features.Events.UpdateEvent
{
    using FluentValidation;

    public class UpdateEventCommandValidator : AbstractValidator<UpdateEventCommand>
    {
        public UpdateEventCommandValidator()
        {
            this.RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Event ID is required.");

            this.RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");

            this.RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MinimumLength(100).WithMessage("Description must be atleast 100 characters.")
                .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");

            this.RuleFor(x => x.Location)
                .NotEmpty().WithMessage("Location is required.")
                .MaximumLength(200).WithMessage("Location must not exceed 200 characters.");

            this.RuleFor(x => x.Capacity)
                .GreaterThan(0).WithMessage("Capacity must be greater than 0.")
                .LessThanOrEqualTo(10000).WithMessage("Capacity must not exceed 10,000.");

            this.RuleFor(x => x.StartDateTime)
                .GreaterThan(DateTime.Now).WithMessage("Start time must be in the future.");

            this.RuleFor(x => x.EndDateTime)
                .GreaterThan(x => x.StartDateTime).WithMessage("End time must be after start time.");
        }
    }
}
