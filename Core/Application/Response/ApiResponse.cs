// <copyright file="ApiResponse.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Response
{

    public static class ApiResponse
    {
        public static ApiResponse<T> Success<T>(T data) =>
            new ApiResponse<T> { Data = data };

        public static ApiResponse<string> Fail(string error) =>
            new ApiResponse<string> { Success = false, Data = error };
    }
}
