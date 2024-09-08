using Bonyan.DDD.Domain.Enumerations;

namespace Nezam.Service.Identity.Domain.Departments;

public class DepartmentStatus : Enumeration
{
    public static readonly DepartmentStatus Active = new(1, nameof(Active));
    public static readonly DepartmentStatus Inactive = new(2, nameof(Inactive));
    public static readonly DepartmentStatus Suspended = new(3, nameof(Suspended));

    public DepartmentStatus(int id, string name) : base(id, name)
    {
    }
}