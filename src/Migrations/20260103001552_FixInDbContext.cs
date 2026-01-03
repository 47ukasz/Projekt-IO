using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projekt_io.Migrations
{
    /// <inheritdoc />
    public partial class FixInDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Animal_AspNetUsers_UserId",
                table: "Animal");

            migrationBuilder.DropForeignKey(
                name: "FK_LostReport_Animal_AnimalId",
                table: "LostReport");

            migrationBuilder.DropForeignKey(
                name: "FK_LostReport_AspNetUsers_UserId",
                table: "LostReport");

            migrationBuilder.DropForeignKey(
                name: "FK_LostReport_Location_LocationId",
                table: "LostReport");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LostReport",
                table: "LostReport");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Location",
                table: "Location");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Animal",
                table: "Animal");

            migrationBuilder.RenameTable(
                name: "LostReport",
                newName: "LostReports");

            migrationBuilder.RenameTable(
                name: "Location",
                newName: "Locations");

            migrationBuilder.RenameTable(
                name: "Animal",
                newName: "Animals");

            migrationBuilder.RenameIndex(
                name: "IX_LostReport_UserId",
                table: "LostReports",
                newName: "IX_LostReports_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_LostReport_LocationId",
                table: "LostReports",
                newName: "IX_LostReports_LocationId");

            migrationBuilder.RenameIndex(
                name: "IX_LostReport_AnimalId",
                table: "LostReports",
                newName: "IX_LostReports_AnimalId");

            migrationBuilder.RenameIndex(
                name: "IX_Animal_UserId",
                table: "Animals",
                newName: "IX_Animals_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LostReports",
                table: "LostReports",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Locations",
                table: "Locations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Animals",
                table: "Animals",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Animals_AspNetUsers_UserId",
                table: "Animals",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LostReports_Animals_AnimalId",
                table: "LostReports",
                column: "AnimalId",
                principalTable: "Animals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LostReports_AspNetUsers_UserId",
                table: "LostReports",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LostReports_Locations_LocationId",
                table: "LostReports",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Animals_AspNetUsers_UserId",
                table: "Animals");

            migrationBuilder.DropForeignKey(
                name: "FK_LostReports_Animals_AnimalId",
                table: "LostReports");

            migrationBuilder.DropForeignKey(
                name: "FK_LostReports_AspNetUsers_UserId",
                table: "LostReports");

            migrationBuilder.DropForeignKey(
                name: "FK_LostReports_Locations_LocationId",
                table: "LostReports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LostReports",
                table: "LostReports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Locations",
                table: "Locations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Animals",
                table: "Animals");

            migrationBuilder.RenameTable(
                name: "LostReports",
                newName: "LostReport");

            migrationBuilder.RenameTable(
                name: "Locations",
                newName: "Location");

            migrationBuilder.RenameTable(
                name: "Animals",
                newName: "Animal");

            migrationBuilder.RenameIndex(
                name: "IX_LostReports_UserId",
                table: "LostReport",
                newName: "IX_LostReport_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_LostReports_LocationId",
                table: "LostReport",
                newName: "IX_LostReport_LocationId");

            migrationBuilder.RenameIndex(
                name: "IX_LostReports_AnimalId",
                table: "LostReport",
                newName: "IX_LostReport_AnimalId");

            migrationBuilder.RenameIndex(
                name: "IX_Animals_UserId",
                table: "Animal",
                newName: "IX_Animal_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LostReport",
                table: "LostReport",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Location",
                table: "Location",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Animal",
                table: "Animal",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Animal_AspNetUsers_UserId",
                table: "Animal",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LostReport_Animal_AnimalId",
                table: "LostReport",
                column: "AnimalId",
                principalTable: "Animal",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LostReport_AspNetUsers_UserId",
                table: "LostReport",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LostReport_Location_LocationId",
                table: "LostReport",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
