// <copyright file="GetUserDetailsQueryHandler.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Features.Auth.GetUserDetails
{
    using EventManagementAPI.Core.Application.Contracts.Identity;
    using EventManagementAPI.Core.Application.Contracts.Messaging.Query;
    using EventManagementAPI.Core.Application.DTO;
    using EventManagementAPI.Core.Application.Response;

    public class GetUserDetailsQueryhandler : IQueryHandler<GetUserDetailsQuery, Result<UserDataDTO>>
    {
        private readonly IUserService userService;

        public GetUserDetailsQueryhandler(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task<Result<UserDataDTO>> Handle(GetUserDetailsQuery request, CancellationToken cancellationToken)
        {
            var userDetails = await this.userService.GetUserDetailsByIdAsync(request.UserId);
            return Result<UserDataDTO>.Success(userDetails!);
        }
    }
}
