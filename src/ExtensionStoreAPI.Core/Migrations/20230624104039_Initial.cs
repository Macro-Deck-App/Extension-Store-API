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
                name: "downloads",
                schema: "extensionstore",
                columns: table => new
                {
                    d_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    d_version = table.Column<string>(type: "text", nullable: false),
                    e_ref = table.Column<int>(type: "integer", nullable: false),
                    d_created_timestamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_downloads", x => x.d_id);
                    table.ForeignKey(
                        name: "FK_downloads_extensions_e_ref",
                        column: x => x.e_ref,
                        principalSchema: "extensionstore",
                        principalTable: "extensions",
                        principalColumn: "e_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "files",
                schema: "extensionstore",
                columns: table => new
                {
                    f_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    f_version = table.Column<string>(type: "text", nullable: false),
                    f_min_api_version = table.Column<int>(type: "integer", nullable: false),
                    f_pkg_filename = table.Column<string>(type: "text", nullable: false),
                    f_icon_filename = table.Column<string>(type: "text", nullable: false),
                    f_readme = table.Column<string>(type: "text", nullable: true),
                    f_file_hash = table.Column<string>(type: "text", nullable: false),
                    f_license_name = table.Column<string>(type: "text", nullable: false),
                    f_license_url = table.Column<string>(type: "text", nullable: false),
                    e_ref = table.Column<int>(type: "integer", nullable: false),
                    f_created_timestamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_files", x => x.f_id);
                    table.ForeignKey(
                        name: "FK_files_extensions_e_ref",
                        column: x => x.e_ref,
                        principalSchema: "extensionstore",
                        principalTable: "extensions",
                        principalColumn: "e_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_downloads_d_created_timestamp",
                schema: "extensionstore",
                table: "downloads",
                column: "d_created_timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_downloads_d_version",
                schema: "extensionstore",
                table: "downloads",
                column: "d_version");

            migrationBuilder.CreateIndex(
                name: "IX_downloads_e_ref",
                schema: "extensionstore",
                table: "downloads",
                column: "e_ref");

            migrationBuilder.CreateIndex(
                name: "IX_extensions_e_category",
                schema: "extensionstore",
                table: "extensions",
                column: "e_category");

            migrationBuilder.CreateIndex(
                name: "IX_extensions_e_package_id",
                schema: "extensionstore",
                table: "extensions",
                column: "e_package_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_extensions_e_type",
                schema: "extensionstore",
                table: "extensions",
                column: "e_type");

            migrationBuilder.CreateIndex(
                name: "IX_files_e_ref",
                schema: "extensionstore",
                table: "files",
                column: "e_ref");

            migrationBuilder.CreateIndex(
                name: "IX_files_f_version",
                schema: "extensionstore",
                table: "files",
                column: "f_version");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "downloads",
                schema: "extensionstore");

            migrationBuilder.DropTable(
                name: "files",
                schema: "extensionstore");

            migrationBuilder.DropTable(
                name: "extensions",
                schema: "extensionstore");
        }
    }
}
