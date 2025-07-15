// <copyright file="Result.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Response
{
    public class Result<T>
    {
        public T? Value { get; private set; }

        public string? Error { get; private set; }

        public bool IsSuccess => this.Error is null;

        private Result(T? value, string? error)
        {
            this.Value = value;
            this.Error = error;
        }

        public static Result<T> Success(T value) => new (value, null);

        public static Result<T> Failure(string error) => new (default, error);
    }
}
