using Bonyan.DDD.Domain.Aggregates;
using Microsoft.AspNetCore.Identity;
using Nezam.Service.Identity.Domain.Departments;

namespace Nezam.Service.Identity.Domain.Users;

public class User : IdentityUser<Guid>, IAggregateRoot<Guid>
{
    private readonly List<Department> _departments = new();

    // سازنده خصوصی برای جلوگیری از ایجاد مستقیم نمونه بدون استفاده از متدهای کارخانه‌ای
    private User()
    {
    }

    // متد سازنده برای ایجاد کاربر جدید
    public User(string firstName, string lastName, string email)
    {
        if (string.IsNullOrEmpty(firstName)) throw new ArgumentException("First name cannot be empty.");
        if (string.IsNullOrEmpty(lastName)) throw new ArgumentException("Last name cannot be empty.");
        if (string.IsNullOrEmpty(email)) throw new ArgumentException("Email cannot be empty.");

        FirstName = firstName;
        LastName = lastName;
        Email = email;
        UserName = email;
        Status = UserStatus.Pending;
    }

    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public IReadOnlyCollection<Department> Departments => _departments.AsReadOnly();

    // وضعیت کاربر
    public UserStatus Status { get; private set; } = UserStatus.Pending;

    // متد برای دریافت کلیدهای این موجودیت
    public object[] GetKeys()
    {
        return new object[] { Id };
    }

    // متد دامنه برای تغییر وضعیت کاربر
    public void ChangeStatus(UserStatus newStatus)
    {
        if (newStatus == null)
            throw new ArgumentNullException(nameof(newStatus), "Status cannot be null.");

        if (newStatus == UserStatus.Pending)
            throw new InvalidOperationException("Cannot revert to pending status.");

        Status = newStatus;
    }

    // متد برای تغییر اطلاعات کاربر
    public void UpdateUserInformation(string firstName, string lastName)
    {
        if (string.IsNullOrEmpty(firstName)) throw new ArgumentException("First name cannot be empty.");
        if (string.IsNullOrEmpty(lastName)) throw new ArgumentException("Last name cannot be empty.");

        FirstName = firstName;
        LastName = lastName;
    }
}