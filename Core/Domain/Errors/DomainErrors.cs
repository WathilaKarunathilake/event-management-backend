// <copyright file="DomainErrors.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Domain.Errors
{
    public static class DomainErrors
    {
        public static class Registration
        {
            public static Error NotFoundForEvent(Guid eventId) =>
                Error.NotFound(
                    "Registration.NotFoundForEvent",
                    $"No registrations found for the event with ID '{eventId}'.");

            public static Error NotFoundForUser(Guid userId) =>
                Error.NotFound(
                    "Registration.NotFoundForUser",
                    $"No registrations found for the user with ID '{userId}'.");

            public static Error AlreadyCanceled() =>
                Error.Failure(
                    "Registration.AlreadyCanceled",
                    "This registration is already canceled");
        }

        public static class Event
        {
            public static Error TitleIsRequired() =>
                Error.Validation("Event.TitleIsRequired", "Title is required.");

            public static Error TitleTooLong() =>
                Error.Validation("Event.TitleTooLong", "Title must not exceed 100 characters.");

            public static Error DescriptionIsRequired() =>
                Error.Validation("Event.DescriptionIsRequired", "Description is required.");

            public static Error DescriptionTooShort() =>
                Error.Validation("Event.DescriptionTooShort", "Description must be at least 100 characters.");

            public static Error DescriptionTooLong() =>
                Error.Validation("Event.DescriptionTooLong", "Description must not exceed 1000 characters.");

            public static Error LocationIsRequired() =>
                Error.Validation("Event.LocationIsRequired", "Location is required.");

            public static Error LocationTooLong() =>
                Error.Validation("Event.LocationTooLong", "Location must not exceed 200 characters.");

            public static Error CapacityMustBePositive() =>
                Error.Validation("Event.CapacityMustBePositive", "Capacity must be greater than 0.");

            public static Error CapacityTooLarge() =>
                Error.Validation("Event.CapacityTooLarge", "Capacity must not exceed 10,000.");

            public static Error StartTimeInPast() =>
                Error.Validation("Event.StartTimeInPast", "Start time must be in the future.");

            public static Error EndTimeBeforeStart() =>
                Error.Validation("Event.EndTimeBeforeStart", "End time must be after start time.");

            public static Error CreatorIdRequired() =>
                Error.Validation("Event.CreatorIdRequired", "Creator ID is required.");

            public static Error NotFound(Guid id) =>
                Error.NotFound("Event.NotFound", $"Event with ID {id} was not found.");
        }

        public static class Auth
        {
            public static Error EmailIsRequired() =>
                Error.Validation("Auth.EmailIsRequired", "Email is required.");

            public static Error EmailInvalidFormat() =>
                Error.Validation("Auth.EmailInvalidFormat", "Invalid email address format.");

            public static Error PasswordIsRequired() =>
                Error.Validation("Auth.PasswordIsRequired", "Password is required.");

            public static Error PasswordTooShort() =>
                Error.Validation("Auth.PasswordTooShort", "Password must be at least 6 characters long.");

            public static Error PhoneNumberIsRequired() =>
                Error.Validation("Auth.PhoneNumberIsRequired", "Phone number is required.");

            public static Error PhoneNumberInvalid() =>
                Error.Validation("Auth.PhoneNumberInvalid", "Phone number must be valid.");

            public static Error NameIsRequired() =>
                Error.Validation("Auth.NameIsRequired", "Name is required.");

            public static Error NameTooLong() =>
                Error.Validation("Auth.NameTooLong", "Name must not exceed 100 characters.");

            public static Error InvalidRole() =>
                Error.Validation("Auth.InvalidRole", "Invalid role.");

            public static Error InvalidCredentials() =>
                Error.Validation("Auth.InvalidCredentials", "The email or password is incorrect.");
        }

        public static class Custom
        {
            public static Error Failure(string error) => Error.Failure("Custom.Failure", error);
        }
    }
}
