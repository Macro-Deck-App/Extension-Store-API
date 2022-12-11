using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MacroDeckExtensionStoreLibrary.Migrations
{
    public partial class ChangedFileName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "ExtensionFiles",
                newName: "PackageFileName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PackageFileName",
                table: "ExtensionFiles",
                newName: "FileName");
        }
    }
}
