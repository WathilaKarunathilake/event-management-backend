// <copyright file="Result.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Response
{
    using EventManagementAPI.Core.Domain.Errors;

    public class Result<T>
    {
        private Result(T? value, Error? error)
        {
            this.Value = value;
            this.Error = error;
        }

        public T? Value { get; private set; }

        public Error? Error { get; private set; }

        public bool IsSuccess => this.Error is null;

        public static Result<T> Success(T value) => new (value, null);

        public static Result<T> Failure(Error error) => new (default, error);
    }
}
