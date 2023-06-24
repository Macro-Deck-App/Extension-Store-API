using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ExtensionStoreAPI.Core.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "extensionstore");

            migrationBuilder.CreateTable(
                name: "extensions",
                schema: "extensionstore",
                columns: table => new
                {
                    e_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    e_package_id = table.Column<string>(type: "text", nullable: false),
                    e_type = table.Column<int>(type: "integer", nullable: false),
                    e_name = table.Column<string>(type: "text", nullable: false),
                    e_category = table.Column<string>(type: "text", nullable: false),
                    e_author = table.Column<string>(type: "text", nullable: false),
                    e_description = table.Column<string>(type: "text", nullable: true),
                    e_github_repository = table.Column<string>(type: "text", nullable: false),
                    e_discord_author_userid = table.Column<string>(type: "text", nullable: true),
                    e_created_timestamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    e_updated_timestamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_extensions", x => x.e_id);
                });

            migrationBuilder.CreateTable(
                name: "extension_downloads",
                schema: "extensionstore",
                columns: table => new
                {
                    d_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    d_version = table.Column<string>(type: "text", nullable: false),
                    d_ext_ref = table.Column<int>(type: "integer", nullable: false),
                    d_created_timestamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_extension_downloads", x => x.d_id);
                    table.ForeignKey(
                        name: "FK_extension_downloads_extensions_d_ext_ref",
                        column: x => x.d_ext_ref,
                        principalSchema: "extensionstore",
                        principalTable: "extensions",
                        principalColumn: "e_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "extension_files",
                schema: "extensionstore",
                columns: table => new
                {
                    ef_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ef_version = table.Column<string>(type: "text", nullable: false),
                    ef_min_api_version = table.Column<int>(type: "integer", nullable: false),
                    ef_pkg_filename = table.Column<string>(type: "text", nullable: false),
                    ef_icon_filename = table.Column<string>(type: "text", nullable: false),
                    ef_readme = table.Column<string>(type: "text", nullable: false),
                    ef_file_hash = table.Column<string>(type: "text", nullable: false),
                    ef_license_name = table.Column<string>(type: "text", nullable: false),
                    ef_license_url = table.Column<string>(type: "text", nullable: false),
                    UploadDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ef_ext_ref = table.Column<int>(type: "integer", nullable: false),
                    ef_created_timestamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_extension_files", x => x.ef_id);
                    table.ForeignKey(
                        name: "FK_extension_files_extensions_ef_ext_ref",
                        column: x => x.ef_ext_ref,
                        principalSchema: "extensionstore",
                        principalTable: "extensions",
                        principalColumn: "e_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_extension_downloads_d_ext_ref",
                schema: "extensionstore",
                table: "extension_downloads",
                column: "d_ext_ref");

            migrationBuilder.CreateIndex(
                name: "IX_extension_files_ef_ext_ref",
                schema: "extensionstore",
                table: "extension_files",
                column: "ef_ext_ref");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "extension_downloads",
                schema: "extensionstore");

            migrationBuilder.DropTable(
                name: "extension_files",
                schema: "extensionstore");

            migrationBuilder.DropTable(
                name: "extensions",
                schema: "extensionstore");
        }
    }
}
