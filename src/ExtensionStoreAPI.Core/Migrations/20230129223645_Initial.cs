using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MacroDeckExtensionStoreLibrary.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "md_extensions",
                columns: table => new
                {
                    extid = table.Column<long>(name: "ext_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    extpackageid = table.Column<string>(name: "ext_package_id", type: "text", nullable: false),
                    exttype = table.Column<int>(name: "ext_type", type: "integer", nullable: false),
                    extname = table.Column<string>(name: "ext_name", type: "text", nullable: false),
                    extcategory = table.Column<string>(name: "ext_category", type: "text", nullable: false),
                    extauthor = table.Column<string>(name: "ext_author", type: "text", nullable: false),
                    extdescription = table.Column<string>(name: "ext_description", type: "text", nullable: true),
                    extgithubrepository = table.Column<string>(name: "ext_github_repository", type: "text", nullable: false),
                    extdiscordauthoruserid = table.Column<string>(name: "ext_discord_author_userid", type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_md_extensions", x => x.extid);
                });

            migrationBuilder.CreateTable(
                name: "md_extension_downloads",
                columns: table => new
                {
                    exdlid = table.Column<long>(name: "exdl_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    exdlversion = table.Column<string>(name: "exdl_version", type: "text", nullable: false),
                    exdltime = table.Column<DateTime>(name: "exdl_time", type: "timestamp without time zone", nullable: false),
                    exdlextref = table.Column<long>(name: "exdl_ext_ref", type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_md_extension_downloads", x => x.exdlid);
                    table.ForeignKey(
                        name: "FK_md_extension_downloads_md_extensions_exdl_ext_ref",
                        column: x => x.exdlextref,
                        principalTable: "md_extensions",
                        principalColumn: "ext_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "md_extension_files",
                columns: table => new
                {
                    extflid = table.Column<long>(name: "extfl_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    extflversion = table.Column<string>(name: "extfl_version", type: "text", nullable: false),
                    extflminapiversion = table.Column<int>(name: "extfl_min_api_version", type: "integer", nullable: false),
                    extflpkgfilename = table.Column<string>(name: "extfl_pkg_filename", type: "text", nullable: false),
                    extfliconfilename = table.Column<string>(name: "extfl_icon_filename", type: "text", nullable: false),
                    extflreadmehtml = table.Column<string>(name: "extfl_readme_html", type: "text", nullable: false),
                    extfldescription = table.Column<string>(name: "extfl_description", type: "text", nullable: false),
                    extflmd5hash = table.Column<string>(name: "extfl_md5_hash", type: "text", nullable: false),
                    extfllicensename = table.Column<string>(name: "extfl_license_name", type: "text", nullable: false),
                    extfllicenseurl = table.Column<string>(name: "extfl_license_url", type: "text", nullable: false),
                    extfluploadtimestamp = table.Column<DateTime>(name: "extfl_upload_timestamp", type: "timestamp without time zone", nullable: false),
                    extflextref = table.Column<long>(name: "extfl_ext_ref", type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_md_extension_files", x => x.extflid);
                    table.ForeignKey(
                        name: "FK_md_extension_files_md_extensions_extfl_ext_ref",
                        column: x => x.extflextref,
                        principalTable: "md_extensions",
                        principalColumn: "ext_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_md_extension_downloads_exdl_ext_ref",
                table: "md_extension_downloads",
                column: "exdl_ext_ref");

            migrationBuilder.CreateIndex(
                name: "IX_md_extension_files_extfl_ext_ref",
                table: "md_extension_files",
                column: "extfl_ext_ref");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "md_extension_downloads");

            migrationBuilder.DropTable(
                name: "md_extension_files");

            migrationBuilder.DropTable(
                name: "md_extensions");
        }
    }
}
