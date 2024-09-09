using Bonyan.Persistence.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Nezam.Service.Identity.Api.Seeders;

public class RoleSeeder : ISeeder
{
    private readonly ILogger<RoleSeeder> _logger;
    private readonly IServiceProvider _serviceProvider;

    public RoleSeeder(IServiceProvider serviceProvider, ILogger<RoleSeeder> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public int Order { get; set; } = 2; // Set priority order

    public async   Task SeedAsync(CancellationToken cancellationToken = default)
    {
        using var scope = _serviceProvider.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        // لیست نقش‌هایی که باید اضافه شوند
        var roles = new List<string> {  "Owner", "Employee", "Engineer" };

        foreach (var roleName in roles)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                role = new IdentityRole<Guid> { Name = roleName };
                var roleResult = await roleManager.CreateAsync(role);
                if (roleResult.Succeeded)
                {
                    _logger.LogInformation("{RoleName} role has been created.", roleName);
                }
                else
                {
                    _logger.LogError("Failed to create {RoleName} role.", roleName);
                    throw new InvalidOperationException($"Failed to create {roleName} role.");
                }
            }
            else
            {
                _logger.LogInformation("{RoleName} role already exists.", roleName);
            }
        }
    }
}