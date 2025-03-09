using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Configurations;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        // Define primary key
        builder.HasKey(b => b.Id);

        // Ensure BookingNumber is unique
        builder.HasIndex(b => b.BookingNumber).IsUnique();

        // Define required fields
        builder.Property(b => b.BookingNumber)
                .IsRequired()
                .HasMaxLength(100);

        builder.Property(b => b.MainGuestFullName)
                .IsRequired();

        // Define relationships
        builder.HasOne(b => b.Room)
                .WithMany()
                .HasForeignKey(b => b.RoomId)
                .OnDelete(DeleteBehavior.Cascade);
    }
}
