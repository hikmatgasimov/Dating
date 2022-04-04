using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DatingApi.Migrations
{
    public partial class NewPasswordAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordHash",
                table: "AppUser",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordSalt",
                table: "AppUser",
                type: "varbinary(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "AppUser");

            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                table: "AppUser");
        }
    }
}
