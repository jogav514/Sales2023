using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sales.API.Migrations
{
    /// <inheritdoc />
    public partial class CityAndState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_State_Countries_CountryID",
                table: "State");

            migrationBuilder.RenameColumn(
                name: "CountryID",
                table: "State",
                newName: "CountryId");

            migrationBuilder.RenameIndex(
                name: "IX_State_CountryID",
                table: "State",
                newName: "IX_State_CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_State_Name_CountryId",
                table: "State",
                columns: new[] { "Name", "CountryId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cities_Name_StateId",
                table: "Cities",
                columns: new[] { "Name", "StateId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_State_Countries_CountryId",
                table: "State",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_State_Countries_CountryId",
                table: "State");

            migrationBuilder.DropIndex(
                name: "IX_State_Name_CountryId",
                table: "State");

            migrationBuilder.DropIndex(
                name: "IX_Cities_Name_StateId",
                table: "Cities");

            migrationBuilder.DropIndex(
                name: "IX_Categories_Name",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "CountryId",
                table: "State",
                newName: "CountryID");

            migrationBuilder.RenameIndex(
                name: "IX_State_CountryId",
                table: "State",
                newName: "IX_State_CountryID");

            migrationBuilder.AddForeignKey(
                name: "FK_State_Countries_CountryID",
                table: "State",
                column: "CountryID",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
