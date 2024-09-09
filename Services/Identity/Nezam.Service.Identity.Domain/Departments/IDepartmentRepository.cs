using Bonyan.DomainDrivenDesign.Domain.Abstractions;

namespace Nezam.Service.Identity.Domain.Departments;

public interface IDepartmentRepository : IRepository<Department,Guid>
{
    
}