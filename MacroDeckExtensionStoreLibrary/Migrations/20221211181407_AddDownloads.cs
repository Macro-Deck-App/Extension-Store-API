using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MacroDeckExtensionStoreLibrary.Migrations
{
    public partial class AddDownloads : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Downloads",
                table: "Extensions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Downloads",
                table: "Extensions");
        }
    }
}
