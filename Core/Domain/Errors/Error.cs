// <copyright file="Error.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Domain.Errors
{
    using EventManagementAPI.Core.Domain.Enums;

    public class Error
    {
        public static readonly Error None = new (string.Empty, string.Empty, ErrorType.None);
        public static readonly Error NullValue = new ("Error.NullValue", "Null value was provided", ErrorType.Failure);

        public Error(string code, string message, ErrorType type)
        {
            this.Code = code;
            this.Message = message;
            this.Type = type;
        }

        public string Code { get; }

        public string Message { get; }

        public ErrorType Type { get; }

        public static Error Failure(string code, string message) =>
            new (code, message, ErrorType.Failure);

        public static Error Validation(string code, string message) =>
            new (code, message, ErrorType.Validation);

        public static Error NotFound(string code, string message) =>
            new (code, message, ErrorType.NotFound);

        public static Error Unauthorized(string code, string message) =>
            new (code, message, ErrorType.Unauthorized);

        public static Error Forbidden(string code, string message) =>
            new (code, message, ErrorType.Forbidden);

        public static Error Conflict(string code, string message) =>
            new (code, message, ErrorType.Conflict);
    }
}
