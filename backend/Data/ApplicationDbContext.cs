using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Backend.Models;
using System;

namespace Backend.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Event> Events { get; set; } = null!;
        public DbSet<Booking> Bookings { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Booking>()
                .HasOne(b => b.Event)
                .WithMany(e => e.Bookings)
                .HasForeignKey(b => b.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Event>(eb =>
            {
                eb.HasKey(e => e.EventId);

                eb.HasData(
                    new Event
                    {
                        EventId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                        Name = "Holiday Concert",
                        Category = "Music",
                        Date = new DateTime(2025, 12, 20, 19, 0, 0, DateTimeKind.Utc),
                        Price = 49.99m,
                        TotalSeats = 200,
                        AvailableSeats = 200
                    },
                    new Event
                    {
                        EventId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                        Name = "Tech Conference",
                        Category = "Conference",
                        Date = new DateTime(2026, 1, 15, 9, 0, 0, DateTimeKind.Utc),
                        Price = 199.00m,
                        TotalSeats = 500,
                        AvailableSeats = 500
                    },
                    new Event
                    {
                        EventId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                        Name = "Comedy Night",
                        Category = "Comedy",
                        Date = new DateTime(2025, 12, 5, 20, 0, 0, DateTimeKind.Utc),
                        Price = 29.50m,
                        TotalSeats = 150,
                        AvailableSeats = 150
                    }
                );
            });

            builder.Entity<Event>()
                .Property(e => e.Price)
                .HasColumnType("decimal(18,2)");

            builder.Entity<Event>()
                .Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Entity<Event>()
                .Property(e => e.Category)
                .IsRequired()
                .HasMaxLength(100);

            builder.Entity<Booking>()
                .Property(b => b.Status)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}