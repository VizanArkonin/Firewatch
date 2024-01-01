using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Firewatch.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddResourcesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ResourceId",
                table: "Telemetry",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ResourceId",
                table: "MonthlyTelemetry",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ResourceId",
                table: "HourlyTelemetry",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ResourceId",
                table: "DailyTelemetry",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Resources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resources", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Resources",
                column: "Id",
                values: new object[]
                {
                    1,
                    2,
                    3
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Resources");

            migrationBuilder.DropColumn(
                name: "ResourceId",
                table: "Telemetry");

            migrationBuilder.DropColumn(
                name: "ResourceId",
                table: "MonthlyTelemetry");

            migrationBuilder.DropColumn(
                name: "ResourceId",
                table: "HourlyTelemetry");

            migrationBuilder.DropColumn(
                name: "ResourceId",
                table: "DailyTelemetry");
        }
    }
}
