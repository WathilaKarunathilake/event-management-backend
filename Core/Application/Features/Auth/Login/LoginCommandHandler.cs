// <copyright file="LoginCommandHandler.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Features.Auth.Login
{
    using EventManagementAPI.Core.Application.Contracts.Identity;
    using EventManagementAPI.Core.Application.Contracts.Messaging.Commands;
    using EventManagementAPI.Core.Application.DTO;
    using EventManagementAPI.Core.Application.Response;
    using EventManagementAPI.Core.Domain.Errors;

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
            var user = await this.userService.CheckPasswordAsync(request.Email!, request.Password!);
            if (!user)
            {
                return Result<AuthDTO>.Failure(DomainErrors.Auth.InvalidCredentials());
            }

            var userDetails = await this.userService.GetUserDetailsFromEmail(request.Email!);

            string token = this.jwtTokenGenerateService.GenerateToken(userDetails.Name!, userDetails.UserId.ToString(), userDetails.Email!, userDetails.Role.ToString() !);
            var refreshToken = this.jwtTokenGenerateService.GenerateRefreshToken();
            var refreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            // Save refresh token to DB
            await this.userService.SaveRefreshTokenAsync(userDetails.UserId.ToString(), refreshToken, refreshTokenExpiry);

            return Result<AuthDTO>.Success(new AuthDTO
            {
                RefreshToken = refreshToken,
                Token = token,
                Message = "User logged in successfully !",
            });
        }
    }
}
