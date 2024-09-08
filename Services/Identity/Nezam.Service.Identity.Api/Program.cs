using Microsoft.EntityFrameworkCore;
using Nezam.Service.Identity.Api.Jobs;
using Nezam.Service.Identity.Api.Seeders;
using Nezam.Service.Identity.Domain.Departments;
using Nezam.Service.Identity.Domain.Tanents;
using Nezam.Service.Identity.Infrastructure.EntityFrameworkCore;
using Nezam.Service.Identity.Infrastructure.EntityFrameworkCore.Repositories;

BonyanApplication
    .CreateBuilder("Identity", "identity", "1.0.0", args)
    .AddFastEndpoints()
    .AddPersistence(c =>
    {
        c.AddEntityFrameworkCore<IdentityAppDbContext>(efCore =>
        {
            efCore.AddRepository<IDepartmentRepository,DepartmentEfRepository>();
            efCore.AddRepository<ITenantRepository,TenantEfRepository>();
        });
        
        c.AddSeed<RoleSeeder>();

    })
    .AddCronJob<UserMigrationJob>("0 */6 * * *")
    .Build()
    .UseFastEndpoints()
    .Run();