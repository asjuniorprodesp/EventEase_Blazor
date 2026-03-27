using EventEase.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace EventEase.Api.Data;

public class EventEaseDbContext(DbContextOptions<EventEaseDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<EventRegistration> EventRegistrations => Set<EventRegistration>();
    public DbSet<UserSession> UserSessions => Set<UserSession>();
    public DbSet<Attendance> Attendance => Set<Attendance>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(e => e.UserId);
            entity.Property(e => e.FullName).HasMaxLength(150).IsRequired();
            entity.Property(e => e.Email).HasMaxLength(150).IsRequired();
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.ToTable("Events");
            entity.HasKey(e => e.EventId);
            entity.Property(e => e.EventName).HasMaxLength(200).IsRequired();
            entity.Property(e => e.EventLocation).HasMaxLength(200).IsRequired();
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<EventRegistration>(entity =>
        {
            entity.ToTable("EventRegistrations");
            entity.HasKey(e => e.RegistrationId);
            entity.HasIndex(e => new { e.UserId, e.EventId }).IsUnique();
            entity.Property(e => e.RegistrationDate)
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAdd();

            entity.HasOne<User>()
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<Event>()
                .WithMany()
                .HasForeignKey(e => e.EventId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<UserSession>(entity =>
        {
            entity.ToTable("UserSessions");
            entity.HasKey(e => e.SessionId);
            entity.Property(e => e.SessionId)
                .HasDefaultValueSql("NEWID()")
                .ValueGeneratedOnAdd();
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAdd();

            entity.HasOne<User>()
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Attendance>(entity =>
        {
            entity.ToTable("Attendance");
            entity.HasKey(e => e.AttendanceId);
            entity.Property(e => e.CheckInTime)
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAdd();

            entity.HasOne<User>()
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<Event>()
                .WithMany()
                .HasForeignKey(e => e.EventId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
