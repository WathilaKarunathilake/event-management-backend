// <copyright file="Program.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
using System.Text;
using EventManagementAPI.API.Extensions;
using EventManagementAPI.Core.Application.Contracts.Identity;
using EventManagementAPI.Core.Application.Contracts.Persistence;
using EventManagementAPI.Core.Application.Profiles;
using EventManagementAPI.Infrastructure.Identity.Context;
using EventManagementAPI.Infrastructure.Identity.Extensions;
using EventManagementAPI.Infrastructure.Identity.Models;
using EventManagementAPI.Infrastructure.Identity.Requirements;
using EventManagementAPI.Infrastructure.Identity.Services;
using EventManagementAPI.Infrastructure.Persistence.Context;
using EventManagementAPI.Infrastructure.Persistence.Repository;
using EventManagementAPI.Infrastructure.Persistence.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var frontendOrigin = "http://localhost:5173";

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", policy =>
            {
                policy
                    .WithOrigins(frontendOrigin)
                    .AllowCredentials()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        builder.Services.AddOpenApi();

        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IJwtTokenGenerateService, JwtTokenGenerateService>();

        builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddDbContext<AppIdentityDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppIdentityDbContext>();

        builder.Services.AddAutoMapper(typeof(MappingProfile));
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(MappingProfile).Assembly);
        });

        // JWT
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var token = context.Request.Cookies["jwt_token"];
                    if (!string.IsNullOrEmpty(token))
                    {
                        context.Token = token;
                    }

                    return Task.CompletedTask;
                },
            };

            // Your existing token validation params...
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                ValidateAudience = true,
                ValidAudience = builder.Configuration["JwtSettings:Audience"],
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"])),
                ValidateIssuerSigningKey = true,
            };
        });

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("ADMIN", policy => policy.RequireRole("ADMIN"));
            options.AddPolicy("PUBLICUSER", policy => policy.RequireRole("PUBLICUSER"));
            options.AddPolicy("OWNERONLY", policy => policy.Requirements.Add(new UserIdMatchRequirment()));
        });

        var app = builder.Build();
        app.UseCors("AllowFrontend");

        app.UseAuthentication();
        app.UseAuthorization();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        //app.UseHttpsRedirection();

        app.RegisterAllEndpointGroups();

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            await SeedData.InitializeAsync(services);
        }

        app.Run();
    }
}
