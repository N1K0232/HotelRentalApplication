using HotelRentalManager.Authentication.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace HotelRentalManager.Authentication;

public sealed class AuthenticationDataContext : IdentityDbContext
{
	public AuthenticationDataContext(DbContextOptions<AuthenticationDataContext> options)
		: base(options)
	{
	}

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);

		Assembly assembly = Assembly.GetExecutingAssembly();
		builder.ApplyConfigurationsFromAssembly(assembly);

		builder.ApplyTrimStringConverter();
	}
}