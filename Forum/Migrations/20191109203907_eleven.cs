using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Forum.Migrations
{
    public partial class eleven : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_AspNetUsers_AddedId",
                table: "Chats");

            migrationBuilder.DropForeignKey(
                name: "FK_Coments_Coments_ParentComentId",
                table: "Coments");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Posts",
                nullable: false,
                defaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "Posts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_AspNetUsers_AddedId",
                table: "Chats",
                column: "AddedId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Coments_Coments_ParentComentId",
                table: "Coments",
                column: "ParentComentId",
                principalTable: "Coments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_AspNetUsers_AddedId",
                table: "Chats");

            migrationBuilder.DropForeignKey(
                name: "FK_Coments_Coments_ParentComentId",
                table: "Coments");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Posts");

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_AspNetUsers_AddedId",
                table: "Chats",
                column: "AddedId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Coments_Coments_ParentComentId",
                table: "Coments",
                column: "ParentComentId",
                principalTable: "Coments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
