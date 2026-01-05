using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projekt_io.Migrations
{
    /// <inheritdoc />
    public partial class SightingPropertiesUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "SeenDate",
                table: "Sightings",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SeenDate",
                table: "Sightings");
        }
    }
}
