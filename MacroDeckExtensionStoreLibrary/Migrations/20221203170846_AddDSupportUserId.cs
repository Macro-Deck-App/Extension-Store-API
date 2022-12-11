using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MacroDeckExtensionStoreLibrary.Migrations
{
    public partial class AddDSupportUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DSupportUserId",
                table: "Extensions",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "UploadDateTime",
                table: "ExtensionFiles",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DSupportUserId",
                table: "Extensions");

            migrationBuilder.DropColumn(
                name: "UploadDateTime",
                table: "ExtensionFiles");
        }
    }
}
