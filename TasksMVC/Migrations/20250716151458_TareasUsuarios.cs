using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TasksMVC.Migrations
{
    /// <inheritdoc />
    public partial class TareasUsuarios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserCreationId",
                table: "Assignments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_UserCreationId",
                table: "Assignments",
                column: "UserCreationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_AspNetUsers_UserCreationId",
                table: "Assignments",
                column: "UserCreationId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_AspNetUsers_UserCreationId",
                table: "Assignments");

            migrationBuilder.DropIndex(
                name: "IX_Assignments_UserCreationId",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "UserCreationId",
                table: "Assignments");
        }
    }
}
