using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BlazorApp1.Shared.Migrations
{
    /// <inheritdoc />
    public partial class DataCheck : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Forecast",
                columns: new[] { "Id", "Date", "Summary", "TemperatureC" },
                values: new object[,]
                {
                    { 1, new DateTime(2022, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chilly", 5 },
                    { 2, new DateTime(2022, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cool", 6 },
                    { 3, new DateTime(2022, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mild", 7 },
                    { 4, new DateTime(2022, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Warm", 8 },
                    { 5, new DateTime(2022, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Balmy", 9 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Forecast",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Forecast",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Forecast",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Forecast",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Forecast",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
