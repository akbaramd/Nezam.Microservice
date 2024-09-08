using Bonyan.DDD.Domain;
using Nezam.Service.Identity.Domain.Tanents;

namespace Nezam.Service.Identity.Infrastructure.EntityFrameworkCore.Repositories;

public class TenantEfRepository :EfCoreRepository<Tenant,Guid,IdentityAppDbContext>, ITenantRepository
{
    public TenantEfRepository(IdentityAppDbContext dbContext) : base(dbContext)
    {
    }
}