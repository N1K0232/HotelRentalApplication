using HotelRentalManager.Authentication.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelRentalManager.Authentication.Configurations;

internal sealed class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(user => user.FirstName)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(user => user.LastName)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(user => user.DateOfBirth)
            .IsRequired();

        builder.Property(user => user.Age)
            .IsRequired();

        builder.Property(user => user.Gender)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(user => user.City)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(user => user.Country)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(user => user.RefreshToken)
            .HasMaxLength(512)
            .IsRequired(false);

        builder.Property(user => user.RefreshTokenExpirationDate)
            .IsRequired(false);
    }
}