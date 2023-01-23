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
                name: "Extensions",
                columns: table => new
                {
                    ExtensionId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PackageId = table.Column<string>(type: "text", nullable: false),
                    ExtensionType = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Author = table.Column<string>(type: "text", nullable: false),
                    GitHubRepository = table.Column<string>(type: "text", nullable: false),
                    DSupportUserId = table.Column<string>(type: "text", nullable: false),
                    Downloads = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Extensions", x => x.ExtensionId);
                });

            migrationBuilder.CreateTable(
                name: "md_extensions",
                columns: table => new
                {
                    extid = table.Column<int>(name: "ext_id", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PackageId = table.Column<string>(type: "text", nullable: false),
                    exttype = table.Column<int>(name: "ext_type", type: "integer", nullable: false),
                    extname = table.Column<string>(name: "ext_name", type: "text", nullable: false),
                    extauthor = table.Column<string>(name: "ext_author", type: "text", nullable: false),
                    extgithubrepository = table.Column<string>(name: "ext_github_repository", type: "text", nullable: false),
                    extdiscordauthoruserid = table.Column<string>(name: "ext_discord_author_userid", type: "text", nullable: false),
                    extdownloads = table.Column<long>(name: "ext_downloads", type: "bigint", nullable: false, defaultValue: 0L)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_md_extensions", x => x.extid);
                });

            migrationBuilder.CreateTable(
                name: "ExtensionFiles",
                columns: table => new
                {
                    ExtensionFileId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Version = table.Column<string>(type: "text", nullable: false),
                    MinAPIVersion = table.Column<int>(type: "integer", nullable: false),
                    PackageFileName = table.Column<string>(type: "text", nullable: false),
                    IconFileName = table.Column<string>(type: "text", nullable: false),
                    DescriptionHtml = table.Column<string>(type: "text", nullable: false),
                    MD5Hash = table.Column<string>(type: "text", nullable: false),
                    LicenseName = table.Column<string>(type: "text", nullable: false),
                    LicenseUrl = table.Column<string>(type: "text", nullable: false),
                    UploadDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExtensionId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtensionFiles", x => x.ExtensionFileId);
                    table.ForeignKey(
                        name: "FK_ExtensionFiles_Extensions_ExtensionId",
                        column: x => x.ExtensionId,
                        principalTable: "Extensions",
                        principalColumn: "ExtensionId");
                });

            migrationBuilder.CreateTable(
                name: "md_extension_files",
                columns: table => new
                {
                    extflid = table.Column<int>(name: "extfl_id", type: "integer", nullable: false),
                    extflversion = table.Column<string>(name: "extfl_version", type: "text", nullable: false),
                    extflminapiversion = table.Column<int>(name: "extfl_min_api_version", type: "integer", nullable: false),
                    extflpkgfilename = table.Column<string>(name: "extfl_pkg_filename", type: "text", nullable: false),
                    extfliconfilename = table.Column<string>(name: "extfl_icon_filename", type: "text", nullable: false),
                    extflreadmehtml = table.Column<string>(name: "extfl_readme_html", type: "text", nullable: false),
                    extflmd5hash = table.Column<string>(name: "extfl_md5_hash", type: "text", nullable: false),
                    extfllicensename = table.Column<string>(name: "extfl_license_name", type: "text", nullable: false),
                    extfllicenseurl = table.Column<string>(name: "extfl_license_url", type: "text", nullable: false),
                    extfluploadtimestamp = table.Column<DateTime>(name: "extfl_upload_timestamp", type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_md_extension_files", x => x.extflid);
                    table.ForeignKey(
                        name: "FK_md_extension_files_md_extensions_extfl_id",
                        column: x => x.extflid,
                        principalTable: "md_extensions",
                        principalColumn: "ext_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExtensionFiles_ExtensionId",
                table: "ExtensionFiles",
                column: "ExtensionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExtensionFiles");

            migrationBuilder.DropTable(
                name: "md_extension_files");

            migrationBuilder.DropTable(
                name: "Extensions");

            migrationBuilder.DropTable(
                name: "md_extensions");
        }
    }
}
