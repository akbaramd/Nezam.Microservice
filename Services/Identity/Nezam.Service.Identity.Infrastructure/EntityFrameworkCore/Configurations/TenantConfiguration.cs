using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nezam.Service.Identity.Domain.Tanents;

namespace Nezam.Service.Identity.Infrastructure.EntityFrameworkCore.Configurations;

public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        // تنظیمات اولیه فیلدهای Tenant
        builder.Property(t => t.Name)
            .HasMaxLength(200)
            .IsRequired()
            .IsUnicode(false); // بهینه‌سازی برای ذخیره رشته‌های غیرفارسی

        // تنظیمات روابط Tenant با Department
        builder.HasMany(t => t.Departments)
            .WithOne(d => d.Tenant)
            .HasForeignKey(d => d.TenantId)
            .OnDelete(DeleteBehavior.Cascade);

        // ایجاد ایندکس بر روی نام برای جستجوی بهینه‌تر
        builder.HasIndex(t => t.Name)
            .HasDatabaseName("IX_Tenant_Name")
            .IsUnique();
    }
}