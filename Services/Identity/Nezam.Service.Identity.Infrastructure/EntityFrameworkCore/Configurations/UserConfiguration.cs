using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nezam.Service.Identity.Domain.Users;

namespace Nezam.Service.Identity.Infrastructure.EntityFrameworkCore.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.FirstName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(u => u.LastName)
            .HasMaxLength(100)
            .IsRequired();

        // Configure any relationships or indexes if needed
        builder.HasIndex(u => u.Email).IsUnique();
    }
}