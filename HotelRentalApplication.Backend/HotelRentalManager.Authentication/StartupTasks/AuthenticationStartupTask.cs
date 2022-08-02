using HotelRentalManager.Authentication.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HotelRentalManager.Authentication.StartupTasks;

public sealed class AuthenticationStartupTask : BackgroundService
{
    private readonly IServiceScope serviceScope;

    private UserManager<ApplicationUser> userManager;
    private RoleManager<ApplicationRole> roleManager;

    private bool disposed;

    public AuthenticationStartupTask(IServiceProvider serviceProvider)
    {
        serviceScope = serviceProvider.CreateScope();
        disposed = false;

        Configure();
    }

    private void Configure()
    {
        userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        ThrowIfDisposed();

        string[] roleNames = new string[] { RoleNames.Administrator, RoleNames.PowerUser, RoleNames.User };
        foreach (string roleName in roleNames)
        {
            bool roleExists = await roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                await roleManager.CreateAsync(new ApplicationRole(roleName));
            }
        }

        var user = new ApplicationUser
        {
            FirstName = "Nicola",
            LastName = "Silvestri",
            DateOfBirth = DateTime.Parse("22/10/2002"),
            Gender = "Male",
            City = "Acquaviva",
            Country = "Repubblica di San Marino",
            Email = "ns.nicolasilvestri@gmail.com",
            PhoneNumber = "3319907702",
            UserName = "N1K0232",
        };

        user.Age = Convert.ToInt32((DateTime.UtcNow.Date - user.DateOfBirth).TotalDays / 365);

        await CheckCreateUserAsync(user, "NicoSilve22!", RoleNames.Administrator, RoleNames.PowerUser, RoleNames.User);

        async Task CheckCreateUserAsync(ApplicationUser user, string password, params string[] roles)
        {
            var dbUser = await userManager.FindByNameAsync(user.UserName);
            if (dbUser == null)
            {
                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRolesAsync(user, roles);
                }
            }
        }
    }

    public override void Dispose()
    {
        Dispose(true);
        base.Dispose();
    }
    private void Dispose(bool disposing)
    {
        if (disposing && !disposed)
        {
            serviceScope.Dispose();
            userManager.Dispose();
            roleManager.Dispose();

            disposed = true;
        }
    }
    private void ThrowIfDisposed()
    {
        if (disposed)
        {
            Type currentType = typeof(AuthenticationStartupTask);
            throw new ObjectDisposedException(currentType.FullName);
        }
    }
}