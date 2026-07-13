// <copyright file="ServiceCollectionExtensions.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.API.Extensions
{
    using System.Text;
    using EventManagementAPI.Core.Application.Behaviours;
    using EventManagementAPI.Core.Application.Contracts.Identity;
    using EventManagementAPI.Core.Application.Contracts.Notification;
    using EventManagementAPI.Core.Application.Contracts.Persistence;
    using EventManagementAPI.Core.Application.Contracts.Utilities;
    using EventManagementAPI.Core.Application.Profiles;
    using EventManagementAPI.Infrastructure.Identity.Context;
    using EventManagementAPI.Infrastructure.Identity.Models;
    using EventManagementAPI.Infrastructure.Identity.Services;
    using EventManagementAPI.Infrastructure.Persistence.Context;
    using EventManagementAPI.Infrastructure.Persistence.Repository;
    using EventManagementAPI.Infrastructure.Persistence.UnitOfWork;
    using EventManagementAPI.Infrastructure.Utils.Services;
    using FluentValidation;
    using MediatR;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.IdentityModel.Tokens;
    using WathilaKarunathilake.Notification;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCorsPolicy(this IServiceCollection services, string frontendOrigin)
        {
            services.AddCors(options =>
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

            return services;
        }

        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            return services;
        }

        public static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddDbContext<AppIdentityDbContext>(options =>
                options.UseSqlServer(connectionString));

            return services;
        }

        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services.AddIdentityCore<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>();

            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<IEventLinkGeneratorService, EventLinkGeneratorService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IIMageUploadService, ImageUploadService>();
            services.AddScoped<IJwtTokenGenerateService, JwtTokenGenerateService>();
            services.AddScoped<IJwtParserService, JwtParserService>();
            services.AddScoped<IQRCodeGeneratorSerivice, QRCodeGeneratorService>();
            services.AddScoped<INotificationSenderService, NotificationSenderService>();

            services.AddValidatorsFromAssemblyContaining<MappingProfile>();

            return services;
        }

        public static IServiceCollection AddMediatRAndAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(MappingProfile).Assembly);
            });

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

            return services;
        }

        public static IServiceCollection AddAuthenticationAndAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var path = context.HttpContext.Request.Path;
                        if (path.StartsWithSegments("/api/auth/refresh"))
                        {
                            return Task.CompletedTask;
                        }

                        var token = context.Request.Cookies["jwt_token"];
                        if (!string.IsNullOrEmpty(token))
                        {
                            context.Token = token;
                        }

                        return Task.CompletedTask;
                    },
                };

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["JwtSettings:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["JwtSettings:Audience"],
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"] !)),
                    ValidateIssuerSigningKey = true,
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ADMIN", policy => policy.RequireRole("ADMIN"));
                options.AddPolicy("PUBLICUSER", policy => policy.RequireRole("PUBLICUSER"));
            });

            return services;
        }

        public static IServiceCollection AddOtherInfrastructure(this IServiceCollection services)
        {
            services.AddNotificationModule();
            return services;
        }
    }
}
