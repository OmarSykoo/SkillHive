using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Users.Domain.Entities;

namespace Modules.Users.Infrastructure.EntityConfig;

public class EmailVerificationTokenConfig : IEntityTypeConfiguration<EmailVerificationToken>
{
    public void Configure(EntityTypeBuilder<EmailVerificationToken> builder)
    {
        builder.Property(t => t.Token).HasMaxLength(36);
        builder.HasKey(t => t.Token);
        builder
            .HasOne(t => t.User)
            .WithOne()
            .HasForeignKey<EmailVerificationToken>(t => t.UserId);
    }

}
