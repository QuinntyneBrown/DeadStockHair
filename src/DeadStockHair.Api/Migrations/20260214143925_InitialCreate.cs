using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeadStockHair.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Retailers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Url = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    DiscoveredAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastCheckedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Retailers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SavedRetailers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    RetailerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    SavedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedRetailers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScanResults",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    RetailersFound = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScanResults", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Retailers_Name",
                table: "Retailers",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Retailers_Url",
                table: "Retailers",
                column: "Url");

            migrationBuilder.CreateIndex(
                name: "IX_SavedRetailers_RetailerId",
                table: "SavedRetailers",
                column: "RetailerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Retailers");

            migrationBuilder.DropTable(
                name: "SavedRetailers");

            migrationBuilder.DropTable(
                name: "ScanResults");
        }
    }
}
