using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nezam.Service.Identity.Domain.Departments;
using Nezam.Service.Identity.Domain.Users;

namespace Nezam.Service.Identity.Infrastructure.EntityFrameworkCore.Configurations;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.Property(d => d.Name)
            .HasMaxLength(200)
            .IsRequired();

        // TenantId is required
        builder.Property(d => d.TenantId)
            .IsRequired();
        
        // تنظیم رابطه چند به چند بین Users و Departments
        builder.HasMany(d => d.Users)
            .WithMany(u => u.Departments)
            .UsingEntity<Dictionary<string, object>>(
                "DepartmentUsers", // نام جدول واسط
                j => j
                    .HasOne<User>() // تنظیم رابطه سمت کاربر
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne<Department>() // تنظیم رابطه سمت دپارتمان
                    .WithMany()
                    .HasForeignKey("DepartmentId")
                    .OnDelete(DeleteBehavior.Cascade),
                j =>
                {
                    // تنظیمات اضافی جدول واسط
                    j.HasKey("UserId", "DepartmentId"); // تعریف کلید اصلی
                    j.HasIndex("UserId"); // ایندکس روی UserId
                    j.HasIndex("DepartmentId"); // ایندکس روی DepartmentId
                });

        // ایندکس برای TenantId جهت بهینه‌سازی جستجو
        builder.HasIndex(d => d.TenantId)
            .HasDatabaseName("IX_Department_TenantId");
    }
}