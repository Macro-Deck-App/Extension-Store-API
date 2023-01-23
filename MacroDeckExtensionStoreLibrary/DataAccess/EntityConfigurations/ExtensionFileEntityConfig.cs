using MacroDeckExtensionStoreLibrary.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MacroDeckExtensionStoreLibrary.DataAccess.EntityConfigurations;

public class ExtensionFileEntityConfig : IEntityTypeConfiguration<ExtensionFileEntity>
{
    private const string TablePrefix = "md_";
    private const string ColumnPrefix = "extfl_";
    
    public void Configure(EntityTypeBuilder<ExtensionFileEntity> builder)
    {
        builder.ToTable(TablePrefix + "extension_files");
        builder.HasKey(e => e.ExtensionFileId);
        builder.Property(p => p.ExtensionFileId)
            .HasColumnName(ColumnPrefix + "id")
            .IsRequired();
        builder.Property(p => p.Version)
            .HasColumnName(ColumnPrefix + "version")
            .IsRequired();
        builder.Property(p => p.MinApiVersion)
            .HasColumnName(ColumnPrefix + "min_api_version")
            .IsRequired();
        builder.Property(p => p.PackageFileName)
            .HasColumnName(ColumnPrefix + "pkg_filename")
            .IsRequired();
        builder.Property(p => p.IconFileName)
            .HasColumnName(ColumnPrefix + "icon_filename")
            .IsRequired();
        builder.Property(p => p.DescriptionHtml)
            .HasColumnName(ColumnPrefix + "readme_html")
            .IsRequired();
        builder.Property(p => p.MD5Hash)
            .HasColumnName(ColumnPrefix + "md5_hash")
            .IsRequired();
        builder.Property(p => p.DescriptionHtml)
            .HasColumnName(ColumnPrefix + "readme_html")
            .IsRequired();
        builder.Property(p => p.LicenseName)
            .HasColumnName(ColumnPrefix + "license_name")
            .IsRequired();
        builder.Property(p => p.LicenseUrl)
            .HasColumnName(ColumnPrefix + "license_url")
            .IsRequired();
        builder.Property(p => p.UploadDateTime)
            .HasColumnName(ColumnPrefix + "upload_timestamp")
            .IsRequired();
    }
}