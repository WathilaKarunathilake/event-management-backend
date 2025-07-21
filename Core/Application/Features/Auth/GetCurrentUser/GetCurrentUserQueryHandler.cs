using EventManagementAPI.Core.Application.Contracts.Messaging.Query;
using EventManagementAPI.Core.Application.DTO;
using EventManagementAPI.Core.Application.Extensions;
using EventManagementAPI.Core.Application.Response;
using EventManagementAPI.Core.Domain.Errors;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementAPI.Core.Application.Features.Auth.GetCurrentUser
{
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
