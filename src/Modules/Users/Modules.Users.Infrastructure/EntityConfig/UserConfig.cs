using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Users.Domain;

namespace Modules.Users.Infrastructure;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.id);
        builder.HasIndex(u => u.Email)
            .IsUnique();
        builder.Property(u => u.Email)
            .IsRequired();
        builder.Property(u => u.FirstName)
            .IsRequired();
        builder.Property(u => u.LastName)
        .IsRequired();
        builder.OwnsOne(u => u.location, loc =>
        {
            loc.Property(p => p.city).IsRequired();
            loc.Property(p => p.state).IsRequired();
            loc.Property(p => p.description).IsRequired();
        });
        builder.Property(u => u.HashedPassword)
            .IsRequired();
        builder.Property(u => u.PhoneNumber)
            .IsRequired();
        builder.Property(u => u.DateOfCreation)
            .IsRequired();
    }

}
