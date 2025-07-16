using EventManagementAPI.Core.Application.Contracts.Identity;
using EventManagementAPI.Core.Application.Contracts.Messaging.Commands;
using EventManagementAPI.Core.Application.DTO;
using EventManagementAPI.Core.Application.Response;
using EventManagementAPI.Core.Domain.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementAPI.Core.Application.Features.Auth.Login
{
    public class LoginCommandHandler : ICommandHandler<LoginCommand, Result<AuthDTO>>
    {
        private readonly IUserService userService;
        private readonly IJwtTokenGenerateService jwtTokenGenerateService;

        public LoginCommandHandler(IUserService userService, IJwtTokenGenerateService jwtTokenGenerateService)
        {
            this.userService = userService;
            this.jwtTokenGenerateService = jwtTokenGenerateService;
        }

        public async Task<Result<AuthDTO>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await userService.CheckPasswordAsync(request.Email, request.Password);
            if (!user)
            {
                return Result<AuthDTO>.Failure("");
            }
            var userDetails = await userService.GetUserDetailsFromEmail(request.Email);

            string token = jwtTokenGenerateService.GenerateToken(userDetails.Name, userDetails.UserId.ToString(), userDetails.Email, userDetails.Role.ToString());
            return Result<AuthDTO>.Success(new AuthDTO
            {
                Token = token,
                Message = "User logged in successfully !",
            });
        }
    }
}
