// <copyright file="HttpResponseExtensions.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.API.Extensions
{
    public static class HttpResponseExtensions
    {
        public static void SetJwtCookie(this HttpResponse response, string token, string refreshToken, bool rememberMe)
        {
            var accessTokenOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Path = "/",
                Expires = rememberMe ? DateTimeOffset.UtcNow.AddDays(7) : null,
            };

            response.Cookies.Append("jwt_token", token, accessTokenOptions);

            if (rememberMe)
            {
                var refreshTokenOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Lax,
                    Path = "/",
                    Expires = rememberMe ? DateTimeOffset.UtcNow.AddDays(7) : null,
                };
                response.Cookies.Append("refresh_token", refreshToken, refreshTokenOptions);
            }
        }
    }
}
