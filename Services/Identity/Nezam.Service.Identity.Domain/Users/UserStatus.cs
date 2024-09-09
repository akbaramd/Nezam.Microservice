
using Bonyan.DomainDrivenDesign.Domain.Enumerations;

namespace Nezam.Service.Identity.Domain.Users;

public class UserStatus : Enumeration
{
    public static readonly UserStatus Active = new(1, nameof(Active));
    public static readonly UserStatus Inactive = new(2, nameof(Inactive));
    public static readonly UserStatus Suspended = new(3, nameof(Suspended));
    public static readonly UserStatus Pending = new(4, nameof(Pending));

    public UserStatus(int id, string name) : base(id, name)
    {
    }
}