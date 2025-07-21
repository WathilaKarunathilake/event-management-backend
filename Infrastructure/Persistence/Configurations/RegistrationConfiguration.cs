// <copyright file="RegistrationConfiguration.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Infrastructure.Persistence.Configurations
{
    using EventManagementAPI.Core.Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class RegistrationConfiguration : IEntityTypeConfiguration<Registration>
    {
        public void Configure(EntityTypeBuilder<Registration> builder)
        {
            builder.ToTable("Registrations");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Id)
                .IsRequired();

            builder.Property(r => r.EventId)
                .IsRequired();

            builder.Property(r => r.UserId)
                .IsRequired();

            builder.Property(r => r.RegisteredAt)
                .IsRequired();

            builder.Property(r => r.RegisterType)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(r => r.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(r => r.Email)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(r => r.PhoneNumber)
                .HasMaxLength(20)
                .IsRequired();

            builder.HasIndex(r => new { r.UserId, r.EventId }).IsUnique();
        }
    }
}
