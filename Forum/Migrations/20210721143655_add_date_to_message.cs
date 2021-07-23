using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Forum.Migrations
{
    public partial class add_date_to_message : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Messages",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8642a250-3c71-4e43-9b9d-090f836c6c08",
                column: "ConcurrencyStamp",
                value: "f456522a-ba7f-4076-8b9a-075c0541a713");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e8c6906e-c1c0-43fa-aa89-034ec2e6961b",
                column: "ConcurrencyStamp",
                value: "aab8a4b0-f365-49cd-b2bd-b9963e536598");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5736d00c-ee3f-4ea8-b965-d5a21642d06a",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a20d04ad-b22a-4a73-b33d-7fff55191ba8", "AQAAAAEAACcQAAAAENCmgMcYrH9ybEODc9xqLo4vhWDySAJ/8FYeQLmAMkgoeTmUuXz+HtccbRO/rAXbEg==", "e1a30cd7-cd08-407b-b414-16cbeec7f8ba" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dde8b42a-591c-46e1-9de9-49be6442583e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "412bc71c-0a39-420a-ba96-ecff3cd60f9d", "AQAAAAEAACcQAAAAEMTlQNu/OoUiPuMdI2vAxnHDlMCBUwYJB7xvMkbcpyTcMzwmq/UTN6HmmRUbioDheA==", "408a24b8-e8d4-4449-85b0-73908e913ad3" });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: "49dd1460-4b04-4249-a32d-282fcf54ff29",
                column: "DateOfEvent",
                value: new DateTime(2021, 7, 22, 17, 36, 54, 289, DateTimeKind.Local).AddTicks(3407));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: "8fa4c18f-1c26-4738-879b-31ce028392ed",
                column: "DateOfEvent",
                value: new DateTime(2021, 7, 22, 17, 36, 54, 293, DateTimeKind.Local).AddTicks(5070));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Messages");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8642a250-3c71-4e43-9b9d-090f836c6c08",
                column: "ConcurrencyStamp",
                value: "07597159-92d4-4d06-924c-dc0679359277");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e8c6906e-c1c0-43fa-aa89-034ec2e6961b",
                column: "ConcurrencyStamp",
                value: "3765315b-5567-409c-82b1-7006b96612b7");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5736d00c-ee3f-4ea8-b965-d5a21642d06a",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5d6e8f92-dc39-4ec8-9622-493579faadb1", "AQAAAAEAACcQAAAAEAA2CCOlgVO6j+QZ++a3fkEHfaFzKJNqEk5KaSG2CbOX6CecVJuA5xJvY2PBsr8Hyw==", "b61fef40-8b24-48fa-b4f1-c67d0f97eb53" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dde8b42a-591c-46e1-9de9-49be6442583e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b1ebb56d-f01e-4f33-8cee-505c29548761", "AQAAAAEAACcQAAAAEJpj8VtNsx6iYg1sFEQ34pULKxp3tGLVssQ1aJ8Acn+ow8PvDgQVKEP4OSbpVGiKcQ==", "1fafa572-29f1-4530-95f5-f931db366fad" });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: "49dd1460-4b04-4249-a32d-282fcf54ff29",
                column: "DateOfEvent",
                value: new DateTime(2021, 7, 15, 18, 55, 52, 580, DateTimeKind.Local).AddTicks(9222));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: "8fa4c18f-1c26-4738-879b-31ce028392ed",
                column: "DateOfEvent",
                value: new DateTime(2021, 7, 15, 18, 55, 52, 583, DateTimeKind.Local).AddTicks(4927));
        }
    }
}
