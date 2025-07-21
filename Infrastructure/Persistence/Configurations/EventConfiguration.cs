// <copyright file="EventConfiguration.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Infrastructure.Persistence.Configurations
{
    using EventManagementAPI.Core.Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.ToTable("Events");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(e => e.Location)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(e => e.Capacity)
                .IsRequired();

            builder.Property(e => e.StartDateTime)
                .IsRequired();

            builder.Property(e => e.EndDateTime)
                .IsRequired();

            builder.Property(e => e.EventType)
                .IsRequired();

            builder.Property(e => e.CreatedBy)
                .IsRequired();

            builder.Property(e => e.EventType)
                .HasConversion<string>();
        }
    }

}
