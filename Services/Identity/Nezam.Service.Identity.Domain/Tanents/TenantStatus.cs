using Bonyan.DomainDrivenDesign.Domain.Enumerations;

namespace Nezam.Service.Identity.Domain.Tanents;

public class TenantStatus : Enumeration
{
    public static readonly TenantStatus Active = new(1, nameof(Active));
    public static readonly TenantStatus Inactive = new(2, nameof(Inactive));
    public static readonly TenantStatus Suspended = new(3, nameof(Suspended));

    public TenantStatus(int id, string name) : base(id, name)
    {
    }
}