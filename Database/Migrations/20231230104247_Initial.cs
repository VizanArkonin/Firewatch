using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Firewatch.Database.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Metrics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    UnitOfMeasurement = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Metrics", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Metrics",
                columns: new[] { "Id", "Description", "Name", "UnitOfMeasurement" },
                values: new object[,]
                {
                    { 1, "Percentage of resource used", "Percent", "%" },
                    { 2, "Amount of Megabytes used by resource", "Megabytes Used", "mB" },
                    { 3, "Total amount of Megabytes available to resource", "Total Megabytes available", "mB" },
                    { 4, "Amount of bytes, sent by given resource", "Bytes sent", "b" },
                    { 5, "Amount of bytes, received by given resource", "Bytes received", "b" },
                    { 6, "The speed of transmission for given resource", "Bytes sent per second", "b/sec" },
                    { 7, "The speed of reception for given resource", "Bytes received per second", "b/sec" },
                    { 8, "Resource temperature", "Temperature", "°C" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Metrics");
        }
    }
}
