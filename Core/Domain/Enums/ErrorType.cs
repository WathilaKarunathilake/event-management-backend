// <copyright file="ErrorType.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Domain.Enums
{
    public enum ErrorType
    {
        Failure,
        Validation,
        Unauthorized,
        NotFound,
        None,
        Forbidden,
        Conflict,
    }
}
