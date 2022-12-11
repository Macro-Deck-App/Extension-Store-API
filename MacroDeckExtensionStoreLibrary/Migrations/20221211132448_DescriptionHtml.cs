using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MacroDeckExtensionStoreLibrary.Migrations
{
    public partial class DescriptionHtml : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DescriptionMarkup",
                table: "ExtensionFiles",
                newName: "DescriptionHtml");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DescriptionHtml",
                table: "ExtensionFiles",
                newName: "DescriptionMarkup");
        }
    }
}
