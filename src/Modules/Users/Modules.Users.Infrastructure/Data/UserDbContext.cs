using Microsoft.EntityFrameworkCore;
using Modules.Users.Application;
using Modules.Users.Domain;
using Modules.Users.Domain.Entities;
using Modules.Users.Infrastructure.EntityConfig;

namespace Modules.Users.Infrastructure;

public class UserDbContext(DbContextOptions<UserDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration<User>(new UserConfig());
        modelBuilder.ApplyConfiguration<RefreshToken>(new RefreshTokenConfig());
    }
}
