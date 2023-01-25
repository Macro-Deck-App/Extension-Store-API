﻿// <auto-generated />
using System;
using MacroDeckExtensionStoreLibrary.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MacroDeckExtensionStoreLibrary.Migrations
{
    [DbContext(typeof(ExtensionStoreDbContext))]
    partial class ExtensionStoreDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MacroDeckExtensionStoreLibrary.DataAccess.Entities.ExtensionDownloadInfoEntity", b =>
                {
                    b.Property<long>("ExtensionDownloadId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("exdl_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("ExtensionDownloadId"));

                    b.Property<DateTime>("DownloadDateTime")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("exdl_time");

                    b.Property<string>("DownloadedVersion")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("exdl_version");

                    b.Property<long>("ExtensionId")
                        .HasColumnType("bigint")
                        .HasColumnName("exdl_ext_ref");

                    b.HasKey("ExtensionDownloadId");

                    b.HasIndex("ExtensionId");

                    b.ToTable("md_extension_downloads", (string)null);
                });

            modelBuilder.Entity("MacroDeckExtensionStoreLibrary.DataAccess.Entities.ExtensionEntity", b =>
                {
                    b.Property<long>("ExtensionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("ext_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("ExtensionId"));

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("ext_author");

                    b.Property<string>("DSupportUserId")
                        .HasColumnType("text")
                        .HasColumnName("ext_discord_author_userid");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("ext_description");

                    b.Property<int>("ExtensionType")
                        .HasColumnType("integer")
                        .HasColumnName("ext_type");

                    b.Property<string>("GitHubRepository")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("ext_github_repository");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("ext_name");

                    b.Property<string>("PackageId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("ext_package_id");

                    b.HasKey("ExtensionId");

                    b.ToTable("md_extensions", (string)null);
                });

            modelBuilder.Entity("MacroDeckExtensionStoreLibrary.DataAccess.Entities.ExtensionFileEntity", b =>
                {
                    b.Property<long>("ExtensionFileId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("extfl_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("ExtensionFileId"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("extfl_description");

                    b.Property<long>("ExtensionId")
                        .HasColumnType("bigint")
                        .HasColumnName("extfl_ext_ref");

                    b.Property<string>("IconFileName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("extfl_icon_filename");

                    b.Property<string>("LicenseName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("extfl_license_name");

                    b.Property<string>("LicenseUrl")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("extfl_license_url");

                    b.Property<string>("Md5Hash")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("extfl_md5_hash");

                    b.Property<int>("MinApiVersion")
                        .HasColumnType("integer")
                        .HasColumnName("extfl_min_api_version");

                    b.Property<string>("PackageFileName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("extfl_pkg_filename");

                    b.Property<string>("ReadmeHtml")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("extfl_readme_html");

                    b.Property<DateTime>("UploadDateTime")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("extfl_upload_timestamp");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("extfl_version");

                    b.HasKey("ExtensionFileId");

                    b.HasIndex("ExtensionId");

                    b.ToTable("md_extension_files", (string)null);
                });

            modelBuilder.Entity("MacroDeckExtensionStoreLibrary.DataAccess.Entities.ExtensionDownloadInfoEntity", b =>
                {
                    b.HasOne("MacroDeckExtensionStoreLibrary.DataAccess.Entities.ExtensionEntity", "ExtensionEntity")
                        .WithMany("Downloads")
                        .HasForeignKey("ExtensionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ExtensionEntity");
                });

            modelBuilder.Entity("MacroDeckExtensionStoreLibrary.DataAccess.Entities.ExtensionFileEntity", b =>
                {
                    b.HasOne("MacroDeckExtensionStoreLibrary.DataAccess.Entities.ExtensionEntity", "ExtensionEntity")
                        .WithMany("ExtensionFiles")
                        .HasForeignKey("ExtensionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ExtensionEntity");
                });

            modelBuilder.Entity("MacroDeckExtensionStoreLibrary.DataAccess.Entities.ExtensionEntity", b =>
                {
                    b.Navigation("Downloads");

                    b.Navigation("ExtensionFiles");
                });
#pragma warning restore 612, 618
        }
    }
}
