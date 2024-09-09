using Bonyan.DomainDrivenDesign.Domain.Aggregates;
using Nezam.Service.Identity.Domain.Tanents;
using Nezam.Service.Identity.Domain.Users;

namespace Nezam.Service.Identity.Domain.Departments;

public class Department : FullAuditableAggregateRoot<Guid>
{
    // لیست کاربران مربوط به این دپارتمان
    private readonly List<User> _users = new();

    // سازنده خصوصی برای جلوگیری از ایجاد مستقیم نمونه
    private Department()
    {
    }

    // سازنده اصلی برای ایجاد دپارتمان
    public Department(string name, Guid tenantId)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentException("Department name cannot be empty.");
        if (tenantId == Guid.Empty)
            throw new ArgumentException("Tenant ID cannot be empty.");

        Id = Guid.NewGuid();
        Name = name;
        TenantId = tenantId;
        Status = DepartmentStatus.Active;
    }

    public string Name { get; private set; }
    public IReadOnlyCollection<User> Users => _users.AsReadOnly();

    // Tenant مربوط به این دپارتمان
    public Guid TenantId { get; private set; }
    public Tenant Tenant { get; }

    // وضعیت دپارتمان
    public DepartmentStatus Status { get; private set; } = DepartmentStatus.Active;

    // متد دامنه برای تغییر نام دپارتمان
    public void ChangeName(string newName)
    {
        if (string.IsNullOrEmpty(newName))
            throw new ArgumentException("New department name cannot be empty.");

        Name = newName;
    }

    // متد دامنه برای تغییر وضعیت دپارتمان
    public void ChangeStatus(DepartmentStatus newStatus)
    {
        if (newStatus == null)
            throw new ArgumentNullException(nameof(newStatus), "Status cannot be null.");

        Status = newStatus;
    }
}