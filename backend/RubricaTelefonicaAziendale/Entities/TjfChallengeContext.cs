using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RubricaTelefonicaAziendale.Entities;

public partial class TjfChallengeContext : DbContext
{
    public TjfChallengeContext()
    {
    }

    public TjfChallengeContext(DbContextOptions<TjfChallengeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Logs> Logs { get; set; }

    public virtual DbSet<Roles> Roles { get; set; }

    public virtual DbSet<UserRoles> UserRoles { get; set; }

    public virtual DbSet<Users> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Logs>(entity =>
        {
            entity.Property(e => e.Endpoint)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.IpAddress)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Message).HasColumnType("text");
            entity.Property(e => e.RawData).HasColumnType("text");
            entity.Property(e => e.Timestamp).HasColumnType("datetime");
            entity.Property(e => e.UsersId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("Users_Id");
        });

        modelBuilder.Entity<Roles>(entity =>
        {
            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UserRoles>(entity =>
        {
            entity.HasKey(e => new { e.UsersId, e.RolesId });

            entity.Property(e => e.UsersId)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.RolesId)
                .HasMaxLength(36)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Users>(entity =>
        {
            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.Firstname)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Lastname)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Password).HasColumnType("text");
            entity.Property(e => e.Picture).HasColumnType("text");
            entity.Property(e => e.Salt).HasColumnType("text");
            entity.Property(e => e.Username)
                .HasMaxLength(500)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
