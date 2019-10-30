using Microsoft.EntityFrameworkCore.Migrations;

namespace Forum.Migrations
{
    public partial class seven : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coments_AspNetUsers_CreatorId",
                table: "Coments");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "Coments",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Coments_CreatorId",
                table: "Coments",
                newName: "IX_Coments_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Coments_AspNetUsers_UserId",
                table: "Coments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coments_AspNetUsers_UserId",
                table: "Coments");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Coments",
                newName: "CreatorId");

            migrationBuilder.RenameIndex(
                name: "IX_Coments_UserId",
                table: "Coments",
                newName: "IX_Coments_CreatorId");

            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "Posts",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Coments_AspNetUsers_CreatorId",
                table: "Coments",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
