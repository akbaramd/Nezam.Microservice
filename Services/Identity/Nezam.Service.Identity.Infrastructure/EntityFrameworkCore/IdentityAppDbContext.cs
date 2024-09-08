using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Nezam.Service.Identity.Domain.Departments;
using Nezam.Service.Identity.Domain.Tanents;
using Nezam.Service.Identity.Domain.Users;
using Nezam.Service.Identity.Infrastructure.EntityFrameworkCore.Configurations;

namespace Nezam.Service.Identity.Infrastructure.EntityFrameworkCore;

public class IdentityAppDbContext(DbContextOptions<IdentityAppDbContext> options)
    : IdentityDbContext<User, IdentityRole<Guid>, Guid>(options)
{
    public DbSet<Department> Departments { get; set; }
    public DbSet<Tenant> Tenants { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Apply configurations for each model
        builder.ApplyConfiguration(new UserConfiguration());
        builder.ApplyConfiguration(new DepartmentConfiguration());
        builder.ApplyConfiguration(new TenantConfiguration());
    }
}