using Bonyan.DomainDrivenDesign.Domain.Aggregates;
using Nezam.Service.Identity.Domain.Departments;

namespace Nezam.Service.Identity.Domain.Tanents;

public class Tenant : FullAuditableAggregateRoot<Guid>
{
    // لیست دپارتمان‌های مربوط به این Tenant
    private readonly List<Department> _departments = new();

    // لیست کاربران مربوط به این Tenant
    private readonly List<Guid> _userIds = new();

    // سازنده خصوصی برای جلوگیری از ساخت نمونه‌ها بدون متدهای مشخص شده
    private Tenant()
    {
    }

    // سازنده Tenant
    public Tenant(string name)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentException("Tenant name cannot be empty.");

        Id = Guid.NewGuid();
        Name = name;
        Status = TenantStatus.Active;
    }

    public string Name { get; private set; }

    // وضعیت Tenant
    public TenantStatus Status { get; private set; } = TenantStatus.Active;
    public IReadOnlyCollection<Guid> UserIds => _userIds.AsReadOnly();
    public IReadOnlyCollection<Department> Departments => _departments.AsReadOnly();

    // متد دامنه برای تغییر نام Tenant
    public void ChangeName(string newName)
    {
        if (string.IsNullOrEmpty(newName))
            throw new ArgumentException("New tenant name cannot be empty.");

        Name = newName;
    }

    // متد دامنه برای تغییر وضعیت Tenant
    public void ChangeStatus(TenantStatus newStatus)
    {
        if (newStatus == null)
            throw new ArgumentNullException(nameof(newStatus), "Status cannot be null.");

        Status = newStatus;
    }

    // متد برای اضافه کردن کاربر به Tenant
    public void AddUser(Guid userId)
    {
        if (_userIds.Contains(userId))
            throw new InvalidOperationException("User is already part of this tenant.");

        _userIds.Add(userId);
    }

    // متد برای حذف کاربر از Tenant
    public void RemoveUser(Guid userId)
    {
        if (!_userIds.Contains(userId))
            throw new InvalidOperationException("User is not part of this tenant.");

        _userIds.Remove(userId);
    }

    // متد برای اضافه کردن دپارتمان به Tenant
    public void AddDepartment(Department department)
    {
        if (_departments.Contains(department))
            throw new InvalidOperationException("Department already exists in this tenant.");

        _departments.Add(department);
    }

    // متد برای حذف دپارتمان از Tenant
    public void RemoveDepartment(Department department)
    {
        if (!_departments.Contains(department))
            throw new InvalidOperationException("Department does not exist in this tenant.");

        _departments.Remove(department);
    }

    // متد برای دریافت کلیدهای این موجودیت
}