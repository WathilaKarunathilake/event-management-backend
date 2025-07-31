// <copyright file="GetCurrentUserQueryHandler.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Features.Auth.GetCurrentUser
{
    using EventManagementAPI.Core.Application.Contracts.Messaging.Query;
    using EventManagementAPI.Core.Application.DTO;
    using EventManagementAPI.Core.Application.Extensions;
    using EventManagementAPI.Core.Application.Response;
    using EventManagementAPI.Core.Domain.Errors;

    public class GetCurrentUserQueryHandler : IQueryHandler<GetCurrentUserQuery, Result<CurrentUserDTO>>
    {
        public Task<Result<CurrentUserDTO>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var user = request.User;

            if (user == null || !user.IsAuthenticated())
            {
                return Task.FromResult(Result<CurrentUserDTO>.Failure(DomainErrors.Auth.NotAuthenticated()));
            }

            var dto = new CurrentUserDTO
            {
                Name = user.GetUserName(),
                Role = user.GetUserRole(),
            };

            return Task.FromResult(Result<CurrentUserDTO>.Success(dto));
        }
    }
}
