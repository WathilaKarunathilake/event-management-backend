// <copyright file="RegistrationCommandHandler.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Features.Auth.Register
{
    using EventManagementAPI.Core.Application.Contracts.Identity;
    using EventManagementAPI.Core.Application.Contracts.Messaging.Commands;
    using EventManagementAPI.Core.Application.DTO;
    using EventManagementAPI.Core.Application.Response;
    using EventManagementAPI.Core.Domain.Errors;

    public class RegistrationCommandHandler : ICommandHandler<RegistrationCommand, Result<AuthDTO>>
    {
        private readonly IUserService userService;
        private readonly IJwtTokenGenerateService jwtTokenGenerateService;

        public RegistrationCommandHandler(IUserService userService, IJwtTokenGenerateService jwtTokenGenerateService)
        {
            this.userService = userService;
            this.jwtTokenGenerateService = jwtTokenGenerateService;
        }


        public async Task<Result<AuthDTO>> Handle(RegistrationCommand request, CancellationToken cancellationToken)
        {
            var result = await this.userService.CreateUserAsync(request.Name, request.Email, request.Password, request.PhoneNumber, request.Role);

            if (!result.Succeeded)
            {
                return Result<AuthDTO>.Failure(DomainErrors.Custom.Failure(result.Errors!));
            }

            await this.userService.AddToRoleAsync(request.Email, request.Role.ToString());

            string token = this.jwtTokenGenerateService.GenerateToken(result.Name, result.UserId.ToString(), result.Email, request.Role.ToString());
            return Result<AuthDTO>.Success(new AuthDTO
            {
                Token = token,
                Message = "User regisration successful !",
            });
        }
    }
}
