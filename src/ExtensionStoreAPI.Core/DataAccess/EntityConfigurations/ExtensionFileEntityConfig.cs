using ExtensionStoreAPI.Core.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExtensionStoreAPI.Core.DataAccess.EntityConfigurations;

public class ExtensionFileEntityConfig : BaseCreatedEntityConfig<ExtensionFileEntity>
{
    public ExtensionFileEntityConfig()
    {
        Table = "files";
        ColumnPrefix = "f_";
    }

    public override void Configure(EntityTypeBuilder<ExtensionFileEntity> builder)
    {
        base.Configure(builder);
        
        builder.ToTable(Table);

        builder.HasIndex(x => x.Version);

        builder.HasIndex(x => x.ExtensionId);
        
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

        builder.Property(p => p.Readme)
            .HasColumnName(ColumnPrefix + "readme");
        
        builder.Property(p => p.FileHash)
            .HasColumnName(ColumnPrefix + "file_hash")
            .IsRequired();
        
        builder.Property(p => p.LicenseName)
            .HasColumnName(ColumnPrefix + "license_name")
            .IsRequired();
        
        builder.Property(p => p.LicenseUrl)
            .HasColumnName(ColumnPrefix + "license_url")
            .IsRequired();

        builder.Property(p => p.ExtensionId)
            .HasColumnName("e_ref");
    }
}