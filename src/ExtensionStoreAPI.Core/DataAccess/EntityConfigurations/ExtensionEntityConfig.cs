using ExtensionStoreAPI.Core.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExtensionStoreAPI.Core.DataAccess.EntityConfigurations;

public class ExtensionEntityConfig : BaseCreatedUpdatedEntityConfig<ExtensionEntity>
{

    public ExtensionEntityConfig()
    {
        Table = "extensions";
        ColumnPrefix = "e_";
    }

    public override void Configure(EntityTypeBuilder<ExtensionEntity> builder)
    {
        base.Configure(builder);
        
        builder.ToTable(Table);

        builder.HasIndex(x => x.PackageId)
            .IsUnique();

        builder.HasIndex(x => x.ExtensionType);
        
        builder.HasIndex(x => x.Category);
        
        builder.Property(p => p.ExtensionType)
            .HasColumnName(ColumnPrefix + "type")
            .IsRequired();
        
        builder.Property(p => p.PackageId)
            .HasColumnName(ColumnPrefix + "package_id")
            .IsRequired();
        
        builder.Property(p => p.Author)
            .HasColumnName(ColumnPrefix + "author")
            .IsRequired();
        
        builder.Property(p => p.Name)
            .HasColumnName(ColumnPrefix + "name")
            .IsRequired();
        
        builder.Property(p => p.Author)
            .HasColumnName(ColumnPrefix + "author")
            .IsRequired();
        
        builder.Property(p => p.Description)
            .HasColumnName(ColumnPrefix + "description");

        builder.Property(p => p.GitHubRepository)
            .HasColumnName(ColumnPrefix + "github_repository")
            .IsRequired();
        
        builder.Property(p => p.DSupportUserId)
            .HasColumnName(ColumnPrefix + "discord_author_userid");
        
        builder.Property(p => p.Category)
            .HasColumnName(ColumnPrefix + "category")
            .IsRequired();

        builder.HasMany(p => p.ExtensionFiles)
            .WithOne(e => e.ExtensionEntity)
            .HasForeignKey(e => e.ExtensionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}