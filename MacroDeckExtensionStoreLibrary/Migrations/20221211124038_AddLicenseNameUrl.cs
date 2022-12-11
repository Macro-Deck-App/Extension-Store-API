using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MacroDeckExtensionStoreLibrary.Migrations
{
    public partial class AddLicenseNameUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "License",
                table: "ExtensionFiles",
                newName: "LicenseUrl");

            migrationBuilder.AddColumn<string>(
                name: "LicenseName",
                table: "ExtensionFiles",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LicenseName",
                table: "ExtensionFiles");

            migrationBuilder.RenameColumn(
                name: "LicenseUrl",
                table: "ExtensionFiles",
                newName: "License");
        }
    }
}
