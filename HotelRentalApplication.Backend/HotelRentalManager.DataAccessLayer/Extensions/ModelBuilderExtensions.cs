using HotelRentalManager.DataAccessLayer.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Reflection;

namespace HotelRentalManager.DataAccessLayer.Extensions;

public static class ModelBuilderExtensions
{
    private static ValueConverter<string, string> trimStringConverter;
    private static MethodInfo setQueryFilter;

    static ModelBuilderExtensions()
    {
        trimStringConverter = new(v => v.Trim(), v => v.Trim());
        setQueryFilter = typeof(DataContext)
            .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
            .Single(t => t.IsGenericMethod && t.Name == "SetQueryFilter");
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

    public static ModelBuilder ApplyQueryFilter(this ModelBuilder modelBuilder, DataContext dataContext)
    {
        var entities = modelBuilder.Model
            .GetEntityTypes()
            .Where(t => typeof(DeletableEntity).IsAssignableFrom(t.ClrType))
            .ToList();

        foreach (var type in entities.Select(t => t.ClrType))
        {
            var methods = SetGlobalQueryMethods(type);

            foreach (var method in methods)
            {
                var genericMethod = method.MakeGenericMethod(type);
                genericMethod.Invoke(dataContext, new object[] { modelBuilder });
            }
        }

        return modelBuilder;
    }

    private static IEnumerable<MethodInfo> SetGlobalQueryMethods(Type type)
    {
        var result = new List<MethodInfo>();

        if (typeof(DeletableEntity).IsAssignableFrom(type))
        {
            result.Add(setQueryFilter);
        }

        return result;
    }
}