using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class SeedDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "EventId", "AvailableSeats", "Category", "Date", "Name", "Price", "TotalSeats" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), 200, "Music", new DateTime(2025, 12, 20, 19, 0, 0, 0, DateTimeKind.Utc), "Holiday Concert", 49.99m, 200 },
                    { new Guid("22222222-2222-2222-2222-222222222222"), 500, "Conference", new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "Tech Conference", 199.00m, 500 },
                    { new Guid("33333333-3333-3333-3333-333333333333"), 150, "Comedy", new DateTime(2025, 12, 5, 20, 0, 0, 0, DateTimeKind.Utc), "Comedy Night", 29.50m, 150 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "EventId",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "EventId",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "EventId",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));
        }
    }
}
