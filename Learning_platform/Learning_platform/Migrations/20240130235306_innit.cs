using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Learning_platform.Migrations
{
    /// <inheritdoc />
    public partial class innit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "28d6f765-43f8-42ca-a142-8c4e9d0341ff");

            migrationBuilder.AddColumn<int>(
                name: "PasswordResetPin",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResetExpires",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "2f692247-43c3-4e7b-9d7d-0cd029052aca");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c585bc47-ce97-47b6-a6d8-f14dbb3e1e01", "a776f96b-511d-4941-930a-4b84a551c30d", "User", "user" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "PasswordResetPin", "ResetExpires" },
                values: new object[] { "8efa11d6-61eb-4010-a964-ab7f97967b92", "AQAAAAIAAYagAAAAELx2GYPDTj53FuuWqUVK9PQSlKWgeBVNgVCrSgc0KRz+c+NXAFkcL5AyUbr2GMko5Q==", null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c585bc47-ce97-47b6-a6d8-f14dbb3e1e01");

            migrationBuilder.DropColumn(
                name: "PasswordResetPin",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ResetExpires",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "de8a3b6c-9767-4980-b27e-2b50f418048f");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "28d6f765-43f8-42ca-a142-8c4e9d0341ff", "06915d7a-d9f7-455d-953c-7f0882ce10a0", "User", "user" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "78d2af24-219f-42db-b546-31dedd7f1909", "AQAAAAIAAYagAAAAEPEJsiHJoCICzulBRjLrN5XMwIPYZjiSDMuEb/YtKe0PopXRyhFKOtAR3DPAnGzPrA==" });
        }
    }
}
