using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WalkersApp.Migrations
{
    /// <inheritdoc />
    public partial class FKnamechange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Walks_Difficulties_DifficultyId",
                table: "Walks");

            migrationBuilder.RenameColumn(
                name: "DifficultyId",
                table: "Walks",
                newName: "HardnessLevel");

            migrationBuilder.RenameIndex(
                name: "IX_Walks_DifficultyId",
                table: "Walks",
                newName: "IX_Walks_HardnessLevel");

            migrationBuilder.AddForeignKey(
                name: "FK_Walks_Difficulties_HardnessLevel",
                table: "Walks",
                column: "HardnessLevel",
                principalTable: "Difficulties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Walks_Difficulties_HardnessLevel",
                table: "Walks");

            migrationBuilder.RenameColumn(
                name: "HardnessLevel",
                table: "Walks",
                newName: "DifficultyId");

            migrationBuilder.RenameIndex(
                name: "IX_Walks_HardnessLevel",
                table: "Walks",
                newName: "IX_Walks_DifficultyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Walks_Difficulties_DifficultyId",
                table: "Walks",
                column: "DifficultyId",
                principalTable: "Difficulties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
