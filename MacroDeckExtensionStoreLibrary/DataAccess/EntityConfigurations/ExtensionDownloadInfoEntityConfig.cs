using MacroDeckExtensionStoreLibrary.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MacroDeckExtensionStoreLibrary.DataAccess.EntityConfigurations;

public class ExtensionDownloadInfoEntityConfig : IEntityTypeConfiguration<ExtensionDownloadInfoEntity>
{
    private const string TablePrefix = "md_";
    private const string ColumnPrefix = "exdl_";
    
    public void Configure(EntityTypeBuilder<ExtensionDownloadInfoEntity> builder)
    {
        builder.ToTable(TablePrefix + "extension_downloads");   
        builder.HasKey(e => e.ExtensionDownloadId);
        builder.Property(p => p.ExtensionDownloadId)
            .HasColumnName(ColumnPrefix + "id");
        builder.Property(p => p.DownloadedVersion)
            .HasColumnName(ColumnPrefix + "version")
            .IsRequired();
        builder.Property(p => p.DownloadDateTime)
            .HasColumnName(ColumnPrefix + "time")
            .IsRequired();
        builder.Property(p => p.ExtensionId)
            .HasColumnName(ColumnPrefix + "ext_ref");
    }
}