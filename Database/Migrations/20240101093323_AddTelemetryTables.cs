using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Firewatch.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddTelemetryTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DailyTelemetry",
                columns: table => new
                {
                    EntryId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MetricId = table.Column<int>(type: "INTEGER", nullable: false),
                    Value = table.Column<float>(type: "REAL", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyTelemetry", x => x.EntryId);
                });

            migrationBuilder.CreateTable(
                name: "HourlyTelemetry",
                columns: table => new
                {
                    EntryId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MetricId = table.Column<int>(type: "INTEGER", nullable: false),
                    Value = table.Column<float>(type: "REAL", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HourlyTelemetry", x => x.EntryId);
                });

            migrationBuilder.CreateTable(
                name: "MonthlyTelemetry",
                columns: table => new
                {
                    EntryId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MetricId = table.Column<int>(type: "INTEGER", nullable: false),
                    Value = table.Column<float>(type: "REAL", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlyTelemetry", x => x.EntryId);
                });

            migrationBuilder.CreateTable(
                name: "Telemetry",
                columns: table => new
                {
                    EntryId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MetricId = table.Column<int>(type: "INTEGER", nullable: false),
                    Value = table.Column<float>(type: "REAL", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Telemetry", x => x.EntryId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyTelemetry");

            migrationBuilder.DropTable(
                name: "HourlyTelemetry");

            migrationBuilder.DropTable(
                name: "MonthlyTelemetry");

            migrationBuilder.DropTable(
                name: "Telemetry");
        }
    }
}
