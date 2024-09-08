using Bonyan.DDD.Domain;
using Nezam.Service.Identity.Domain.Departments;
using Nezam.Service.Identity.Domain.Tanents;

namespace Nezam.Service.Identity.Infrastructure.EntityFrameworkCore.Repositories;

public class DepartmentEfRepository :EfCoreRepository<Department,Guid,IdentityAppDbContext>, IDepartmentRepository
{
    public DepartmentEfRepository(IdentityAppDbContext dbContext) : base(dbContext)
    {
    }
}