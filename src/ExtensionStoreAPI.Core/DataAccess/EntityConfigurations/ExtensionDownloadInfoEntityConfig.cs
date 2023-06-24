using ExtensionStoreAPI.Core.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExtensionStoreAPI.Core.DataAccess.EntityConfigurations;

public class ExtensionDownloadInfoEntityConfig : BaseCreatedEntityConfig<ExtensionDownloadInfoEntity>
{
    public ExtensionDownloadInfoEntityConfig()
    {
        Table = "extension_downloads";
        ColumnPrefix = "d_";
    }

    public override void Configure(EntityTypeBuilder<ExtensionDownloadInfoEntity> builder)
    {
        base.Configure(builder);
        
        builder.ToTable(Table, schema: Schema);   
        
        builder.Property(p => p.DownloadedVersion)
            .HasColumnName(ColumnPrefix + "version")
            .IsRequired();
        
        builder.Property(p => p.ExtensionId)
            .HasColumnName(ColumnPrefix + "ext_ref");
    }
}