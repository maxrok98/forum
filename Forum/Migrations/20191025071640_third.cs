using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Forum.Migrations
{
    public partial class third : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_PostImage_ImageId1",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Threads_ThreadImage_ImageId1",
                table: "Threads");

            migrationBuilder.DropForeignKey(
                name: "FK_UserImage_AspNetUsers_UserId",
                table: "UserImage");

            migrationBuilder.DropIndex(
                name: "IX_UserImage_UserId",
                table: "UserImage");

            migrationBuilder.DropIndex(
                name: "IX_Threads_ImageId1",
                table: "Threads");

            migrationBuilder.DropIndex(
                name: "IX_Posts_ImageId1",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserImage");

            migrationBuilder.DropColumn(
                name: "ImageId1",
                table: "Threads");

            migrationBuilder.DropColumn(
                name: "ThreadId",
                table: "ThreadImage");

            migrationBuilder.DropColumn(
                name: "ImageId1",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "PostImage");

            migrationBuilder.AlterColumn<string>(
                name: "ImageId",
                table: "Threads",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ImageId",
                table: "Posts",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ImageId",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.InsertData(
                table: "Threads",
                columns: new[] { "Id", "Description", "ImageId", "Name" },
                values: new object[] { "sadfasdfa", "Thread about computer science", null, "CS" });

            migrationBuilder.InsertData(
                table: "Threads",
                columns: new[] { "Id", "Description", "ImageId", "Name" },
                values: new object[] { "teewrsl", "Thread about electrical engeneering", null, "Electrical engeneering" });

            migrationBuilder.CreateIndex(
                name: "IX_Threads_ImageId",
                table: "Threads",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_ImageId",
                table: "Posts",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ImageId",
                table: "AspNetUsers",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_UserImage_ImageId",
                table: "AspNetUsers",
                column: "ImageId",
                principalTable: "UserImage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_PostImage_ImageId",
                table: "Posts",
                column: "ImageId",
                principalTable: "PostImage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Threads_ThreadImage_ImageId",
                table: "Threads",
                column: "ImageId",
                principalTable: "ThreadImage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_UserImage_ImageId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_PostImage_ImageId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Threads_ThreadImage_ImageId",
                table: "Threads");

            migrationBuilder.DropIndex(
                name: "IX_Threads_ImageId",
                table: "Threads");

            migrationBuilder.DropIndex(
                name: "IX_Posts_ImageId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ImageId",
                table: "AspNetUsers");

            migrationBuilder.DeleteData(
                table: "Threads",
                keyColumn: "Id",
                keyValue: "sadfasdfa");

            migrationBuilder.DeleteData(
                table: "Threads",
                keyColumn: "Id",
                keyValue: "teewrsl");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "UserImage",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ImageId",
                table: "Threads",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageId1",
                table: "Threads",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThreadId",
                table: "ThreadImage",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ImageId",
                table: "Posts",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageId1",
                table: "Posts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostId",
                table: "PostImage",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ImageId",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserImage_UserId",
                table: "UserImage",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Threads_ImageId1",
                table: "Threads",
                column: "ImageId1");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_ImageId1",
                table: "Posts",
                column: "ImageId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_PostImage_ImageId1",
                table: "Posts",
                column: "ImageId1",
                principalTable: "PostImage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Threads_ThreadImage_ImageId1",
                table: "Threads",
                column: "ImageId1",
                principalTable: "ThreadImage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserImage_AspNetUsers_UserId",
                table: "UserImage",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
