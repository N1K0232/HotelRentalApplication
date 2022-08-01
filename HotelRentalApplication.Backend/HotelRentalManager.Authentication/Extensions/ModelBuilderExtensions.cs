using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HotelRentalManager.Authentication.Extensions;

public static class ModelBuilderExtensions
{
    private static readonly ValueConverter<string, string> trimStringConverter;

    static ModelBuilderExtensions()
    {
        trimStringConverter = new ValueConverter<string, string>(v => v.Trim(), v => v.Trim());
    }

    public static ModelBuilder ApplyTrimStringConverter(this ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(string))
                {
                    modelBuilder.Entity(entityType.Name)
                        .Property(property.Name)
                        .HasConversion(trimStringConverter);
                }
            }
        }

        return modelBuilder;
    }
}