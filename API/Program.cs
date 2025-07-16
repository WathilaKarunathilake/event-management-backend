// <copyright file="Program.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
using EventManagementAPI.API.Extensions;
using EventManagementAPI.Core.Application.Contracts.Identity;
using EventManagementAPI.Core.Application.Contracts.Persistence;
using EventManagementAPI.Core.Application.Profiles;
using EventManagementAPI.Infrastructure.Identity.Context;
using EventManagementAPI.Infrastructure.Identity.Models;
using EventManagementAPI.Infrastructure.Identity.Services;
using EventManagementAPI.Infrastructure.Persistence.Context;
using EventManagementAPI.Infrastructure.Persistence.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddOpenApi();
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

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.RegisterAllEndpointGroups();
        app.Run();
    }
}
