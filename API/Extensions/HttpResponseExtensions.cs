// <copyright file="HttpResponseExtensions.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.API.Extensions
{
    public static class HttpResponseExtensions
    {
        public static void SetJwtCookie(this HttpResponse response, string token, bool rememberMe)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Path = "/",
                Expires = rememberMe ? DateTimeOffset.UtcNow.AddDays(7) : null,
            };

            response.Cookies.Append("jwt_token", token, cookieOptions);
        }
    }
}
