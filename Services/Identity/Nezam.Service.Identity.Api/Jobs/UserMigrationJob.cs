using Bonyan.AspNetCore.Jobs;
using Cronos;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Nezam.Service.Identity.Domain.Users;
using Nezam.Service.Identity.Infrastructure.EntityFrameworkCore;

namespace Nezam.Service.Identity.Api.Jobs;

public class UserMigrationJob(
    ILogger<UserMigrationJob> logger,
    IServiceProvider serviceProvider,
    string? legacyConnectionString)
    : IJob
{
    private Timer? _timer;

    public void Dispose()
    {
        _timer?.Dispose();
    }

    public async Task ExecuteAsync()
    {
        logger.LogInformation("Waiting for data seeding to complete...");
        
        await MigrateUsersAsync();
    }




    private async Task MigrateUsersAsync()
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IdentityAppDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        // Ensure required roles exist
        await EnsureRoleExists(roleManager, "Employer");
        await EnsureRoleExists(roleManager, "Employee");

        await using var connection = new SqlConnection(legacyConnectionString);
        var legacyUsers = await connection.QueryAsync("SELECT * FROM tbl_karbaran");

        foreach (var legacyUser in legacyUsers)
        {
            var normalizedUserName = userManager.NormalizeName(legacyUser.kname);
            var normalizedEmail = userManager.NormalizeEmail($"{legacyUser.kname}@wa-nezam.org");

            User? existingUser = await userManager.FindByNameAsync(normalizedUserName) ??
                                 await userManager.FindByEmailAsync(normalizedEmail);

            if (existingUser == null)
            {
                // کاربر جدید بر اساس DDD
                User newUser = CreateNewUserFromLegacy(legacyUser);

                // ثبت کاربر جدید
                IdentityResult createResult = await userManager.CreateAsync(newUser, legacyUser.pwd);
                if (createResult.Succeeded)
                {
                    await AssignRolesToUser(userManager, newUser, new[] { "Employee" });
                    logger.LogInformation("User {UserName} created and assigned to roles.", newUser.UserName);
                }
                else
                {
                    logger.LogError("Failed to create user {UserName}: {Errors}", newUser.UserName,
                        string.Join(", ", createResult.Errors.Select(e => e.Description)));
                }
            }
            
        }

        logger.LogInformation("User migration completed at {time}", DateTimeOffset.Now);
    }

    private async Task EnsureRoleExists(RoleManager<IdentityRole<Guid>> roleManager, string roleName)
    {
        var role = await roleManager.FindByNameAsync(roleName);
        if (role == null)
        {
            var result = await roleManager.CreateAsync(new IdentityRole<Guid> { Name = roleName });
            if (!result.Succeeded)
            {
                logger.LogError("Failed to create role {RoleName}: {Errors}", roleName,
                    string.Join(", ", result.Errors.Select(e => e.Description)));
                throw new InvalidOperationException($"Failed to create {roleName} role.");
            }
            logger.LogInformation("Role {RoleName} has been created.", roleName);
        }
    }

    private async Task AssignRolesToUser(UserManager<User> userManager, User user, string[] roles)
    {
        foreach (var role in roles)
        {
            if (!await userManager.IsInRoleAsync(user, role))
            {
                var result = await userManager.AddToRoleAsync(user, role);
                if (!result.Succeeded)
                {
                    logger.LogError("Failed to assign role {RoleName} to user {UserName}: {Errors}", role, user.UserName,
                        string.Join(", ", result.Errors.Select(e => e.Description)));
                }
                else
                {
                    logger.LogInformation("User {UserName} assigned to role {RoleName}.", user.UserName, role);
                }
            }
        }
    }

    private User CreateNewUserFromLegacy(dynamic legacyUser)
    {
        var nameParts = legacyUser.kname.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        string firstName = nameParts.Length > 0 ? nameParts[0] : legacyUser.kname;
        string lastName = nameParts.Length > 1 ? string.Join(' ', nameParts.Skip(1)) : string.Empty;

        var email = $"{legacyUser.kname}@wa-nezam.org".ToLowerInvariant();

        // ایجاد کاربر جدید با استفاده از متد دامنه DDD
        return new User(firstName, lastName, email);
    }

 

    private TimeSpan GetNextCronInterval(CronExpression cronExpression)
    {
        var now = DateTime.UtcNow;
        var nextOccurrence = cronExpression.GetNextOccurrence(now, TimeZoneInfo.Utc);
        return nextOccurrence.HasValue ? nextOccurrence.Value - now : TimeSpan.FromHours(12);
    }
}
