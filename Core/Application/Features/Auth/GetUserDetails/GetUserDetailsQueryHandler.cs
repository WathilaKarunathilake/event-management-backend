// <copyright file="GetUserDetailsQueryHandler.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Features.Auth.GetUserDetails
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using EventManagementAPI.Core.Application.Contracts.Identity;
    using EventManagementAPI.Core.Application.Contracts.Messaging.Query;
    using EventManagementAPI.Core.Application.DTO;
    using EventManagementAPI.Core.Application.Extensions;
    using EventManagementAPI.Core.Application.Response;
    using EventManagementAPI.Core.Domain.Errors;

    public class GetUserDetailsQueryHandler : IQueryHandler<GetUserDetailsQuery, Result<UserDataDTO>>
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;

        public GetUserDetailsQueryHandler(IUserService userService, IMapper mapper)
        {
            this.userService = userService;
            this.mapper = mapper;
        }

        public async Task<Result<UserDataDTO>> Handle(GetUserDetailsQuery request, CancellationToken cancellationToken)
        {
            var user = request.User;

            if (user == null || !user.IsAuthenticated())
            {
                return Result<UserDataDTO>.Failure(DomainErrors.Auth.NotAuthenticated());
            }

            var id = user.GetUserId();

            var userDetails = await this.userService.GetUserDetailsByIdAsync(Guid.Parse(id));
            if (userDetails == null)
            {
                return Result<UserDataDTO>.Failure(DomainErrors.Auth.UserNotFound(Guid.Parse(id)));
            }

            return Result<UserDataDTO>.Success(this.mapper.Map<UserDataDTO>(userDetails));
        }
    }
}
