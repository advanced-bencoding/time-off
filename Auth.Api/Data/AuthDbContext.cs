using Auth.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Auth.Api.Data;

public class AuthDbContext(DbContextOptions<AuthDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");

            entity.HasKey(u => u.UserId);

            entity
                .HasIndex(u => u.Email)
                .IsUnique();

            entity
                .Property(u => u.UserId)
                .HasColumnName("user_id");

            entity
                .Property(u => u.FirstName)
                .HasColumnName("first_name")
                .IsRequired();
            entity
                .Property(u => u.MiddleName)
                .HasColumnName("middle_name");
            entity
                .Property(u => u.LastName)
                .HasColumnName("last_name")
                .IsRequired();
            entity
                .Property(u => u.Email)
                .HasColumnName("email")
                .IsRequired();
            entity
                .Property(u => u.PasswordHash)
                .HasColumnName("password_hash")
                .IsRequired();
        });
    }
}
