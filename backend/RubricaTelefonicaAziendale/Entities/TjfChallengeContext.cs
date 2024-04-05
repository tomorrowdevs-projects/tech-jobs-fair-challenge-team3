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

    public virtual DbSet<ContactTypes> ContactTypes { get; set; }

    public virtual DbSet<Contacts> Contacts { get; set; }

    public virtual DbSet<Groups> Groups { get; set; }

    public virtual DbSet<Logs> Logs { get; set; }

    public virtual DbSet<People> People { get; set; }

    public virtual DbSet<Roles> Roles { get; set; }

    public virtual DbSet<Users> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ContactTypes>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ContactType");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Type)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Validation).HasColumnType("text");
        });

        modelBuilder.Entity<Contacts>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Contact)
                .HasMaxLength(1000)
                .IsUnicode(false);

            entity.HasOne(d => d.ContactType).WithMany(p => p.Contacts)
                .HasForeignKey(d => d.ContactTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Contacts_ContactTypes");
        });

        modelBuilder.Entity<Groups>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasMany(d => d.Person).WithMany(p => p.Group)
                .UsingEntity<Dictionary<string, object>>(
                    "PeopleGroups",
                    r => r.HasOne<People>().WithMany()
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_PeopleGroups_People"),
                    l => l.HasOne<Groups>().WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_PeopleGroups_Groups"),
                    j =>
                    {
                        j.HasKey("GroupId", "PersonId");
                    });
        });

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

        modelBuilder.Entity<People>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Firstname)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Lastname)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Picture)
                .HasColumnType("text")
                .HasColumnName("picture");

            entity.HasMany(d => d.Contact).WithMany(p => p.Person)
                .UsingEntity<Dictionary<string, object>>(
                    "PeopleContacts",
                    r => r.HasOne<Contacts>().WithMany()
                        .HasForeignKey("ContactId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_PeopleContacts_Contacts"),
                    l => l.HasOne<People>().WithMany()
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_PeopleContacts_People"),
                    j =>
                    {
                        j.HasKey("PersonId", "ContactId");
                    });
        });

        modelBuilder.Entity<Roles>(entity =>
        {
            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Users>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
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

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_Roles");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
