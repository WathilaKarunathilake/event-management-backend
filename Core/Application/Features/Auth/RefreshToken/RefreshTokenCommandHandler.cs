// <copyright file="RefreshTokenCommandHandler.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Features.Auth.RefreshToken
{
    using System.Threading;
    using System.Threading.Tasks;
    using EventManagementAPI.Core.Application.Contracts.Identity;
    using EventManagementAPI.Core.Application.Contracts.Messaging.Commands;
    using EventManagementAPI.Core.Application.DTO;
    using EventManagementAPI.Core.Application.Response;
    using EventManagementAPI.Core.Domain.Errors;

    public class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, Result<AuthDTO>>
    {
        private readonly IUserService userService;
        private readonly IJwtParserService jwtParser;
        private readonly IJwtTokenGenerateService jwtTokenGenerateService;

        public RefreshTokenCommandHandler(IUserService userService, IJwtParserService jwtParser, IJwtTokenGenerateService jwtTokenGenerateService)
        {
            this.userService = userService;
            this.jwtParser = jwtParser;
            this.jwtTokenGenerateService = jwtTokenGenerateService;
        }

        public async Task<Result<AuthDTO>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var userId = this.jwtParser.ExtractUserId(request.JwtToken!);

            var isValid = await this.userService.ValidateRefreshTokenAsync(userId!, request.RefreshToken!);
            if (!isValid)
            {
                return Result<AuthDTO>.Failure(DomainErrors.Auth.InvalidRefreshToken());
            }

            var user = await this.userService.GetUserDetailsByIdAsync(Guid.Parse(userId!));
            string newAccessToken = this.jwtTokenGenerateService.GenerateToken(
                user!.Name!,
                user.UserId.ToString(),
                user.Email!,
                user.Role.ToString() !);

            // Generate new refresh token and save it
            var newRefreshToken = this.jwtTokenGenerateService.GenerateRefreshToken();
            var refreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            await this.userService.SaveRefreshTokenAsync(user.UserId.ToString(), newRefreshToken, refreshTokenExpiry);

            return Result<AuthDTO>.Success(new AuthDTO
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken,
                Message = "Token refreshed successfully.",
            });
        }
    }
}
