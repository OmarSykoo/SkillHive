using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Users.Domain.Entities;

namespace Modules.Users.Infrastructure.EntityConfig;

public class UnverifiedUserConfig : IEntityTypeConfiguration<UnverifiedUser>
{
    public void Configure(EntityTypeBuilder<UnverifiedUser> builder)
    {
        builder.HasIndex(e => e.Email);
        builder.OwnsOne(u => u.location, loc =>
        {
            loc.Property(p => p.city).IsRequired();
            loc.Property(p => p.state).IsRequired();
            loc.Property(p => p.description).IsRequired();
        });
    }
}
