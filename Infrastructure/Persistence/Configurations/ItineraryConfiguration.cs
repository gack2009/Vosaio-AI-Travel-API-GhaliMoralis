using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Configurations;

public class ItineraryConfiguration : IEntityTypeConfiguration<Itinerary>
{
    public void Configure(EntityTypeBuilder<Itinerary> builder)
    {
        // Define primary key
        builder.HasKey(b => b.Id);

        // Define required fields
        builder.Property(b => b.Destination)
                .IsRequired()
                .HasMaxLength(100);

        builder.Property(b => b.ItineraryJson)
                .IsRequired()
                .HasMaxLength(500);

        builder.Property(b => b.GeneratedAt)
                .IsRequired();
    }
}
