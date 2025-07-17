// <copyright file="ApiResponse.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Response
{
    using EventManagementAPI.Core.Domain.Errors;

    public static class ApiResponse
    {
        public static ApiResponse<T> Success<T>(T data) =>
            new ApiResponse<T> { Data = data };

        public static ApiResponse<Error> Fail(Error error) =>
            new ApiResponse<Error> { Success = false, Data = error };
    }
}
