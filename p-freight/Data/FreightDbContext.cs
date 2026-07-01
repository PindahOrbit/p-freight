using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using p_freight.Models;

namespace p_freight.Data;

public partial class FreightDbContext : DbContext
{
    public FreightDbContext()
    {
    }

    public FreightDbContext(DbContextOptions<FreightDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DriverProfile> DriverProfiles { get; set; }

    public virtual DbSet<Load> Loads { get; set; }

    public virtual DbSet<LoadBid> LoadBids { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<Organisation> Organisations { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RoleClaim> RoleClaims { get; set; }

    public virtual DbSet<Trip> Trips { get; set; }

    public virtual DbSet<TripLocation> TripLocations { get; set; }

    public virtual DbSet<Truck> Trucks { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserClaim> UserClaims { get; set; }

    public virtual DbSet<UserLogin> UserLogins { get; set; }

    public virtual DbSet<UserToken> UserTokens { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite("name=freightConn");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DriverProfile>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.IsOnline).HasDefaultValue(false);
            entity.Property(e => e.IsVerified).HasDefaultValue(false);
            entity.Property(e => e.Rating).HasDefaultValue(0.0);
            entity.Property(e => e.TotalTrips).HasDefaultValue(0);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Organisation).WithMany(p => p.DriverProfiles).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.User).WithOne(p => p.DriverProfile).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Load>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Status).HasDefaultValue("PENDING");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Customer).WithMany(p => p.Loads).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Organisation).WithMany(p => p.Loads).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<LoadBid>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Currency).HasDefaultValue("USD");
            entity.Property(e => e.Status).HasDefaultValue("PENDING");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Driver).WithMany(p => p.LoadBids).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Load).WithMany(p => p.LoadBids).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Organisation).WithMany(p => p.LoadBids).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.IsRead).HasDefaultValue(false);

            entity.HasOne(d => d.Load).WithMany(p => p.Messages).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Organisation).WithMany(p => p.Messages).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Receiver).WithMany(p => p.MessageReceivers).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Sender).WithMany(p => p.MessageSenders).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Organisation>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Currency).HasDefaultValue("USD");
            entity.Property(e => e.Status).HasDefaultValue("PENDING");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Organisation).WithMany(p => p.Payments).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Payee).WithMany(p => p.PaymentPayees).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Payer).WithMany(p => p.PaymentPayers).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Trip).WithMany(p => p.Payments).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Organisation).WithMany(p => p.Reviews).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Reviewee).WithMany(p => p.ReviewReviewees).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Reviewer).WithMany(p => p.ReviewReviewers).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Trip).WithMany(p => p.Reviews).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Trip>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Currency).HasDefaultValue("USD");
            entity.Property(e => e.Status).HasDefaultValue("SCHEDULED");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Driver).WithMany(p => p.Trips).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Load).WithMany(p => p.Trips).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Organisation).WithMany(p => p.Trips).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Truck).WithMany(p => p.Trips).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<TripLocation>(entity =>
        {
            entity.Property(e => e.RecordedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Organisation).WithMany(p => p.TripLocations).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Trip).WithMany(p => p.TripLocations).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Truck>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.IsVerified).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Organisation).WithMany(p => p.Trucks).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Owner).WithMany(p => p.Trucks).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserRole",
                    r => r.HasOne<Role>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<User>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("user_roles");
                        j.HasIndex(new[] { "RoleId" }, "IX_user_roles_RoleId");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
