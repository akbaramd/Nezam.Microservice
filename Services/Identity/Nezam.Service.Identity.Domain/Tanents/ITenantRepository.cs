using Bonyan.DomainDrivenDesign.Domain.Abstractions;

namespace Nezam.Service.Identity.Domain.Tanents;

public interface ITenantRepository : IRepository<Tenant,Guid>
{
    
}