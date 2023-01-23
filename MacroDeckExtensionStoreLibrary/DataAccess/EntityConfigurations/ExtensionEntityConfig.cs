using MacroDeckExtensionStoreLibrary.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MacroDeckExtensionStoreLibrary.DataAccess.EntityConfigurations;

public class ExtensionEntityConfig : IEntityTypeConfiguration<ExtensionEntity>
{
    private const string TablePrefix = "md_";
    private const string ColumnPrefix = "ext_";

    public void Configure(EntityTypeBuilder<ExtensionEntity> builder)
    {
        builder.ToTable(TablePrefix + "extensions");   
        builder.HasKey(e => e.ExtensionId);
        builder.Property(p => p.ExtensionId)
            .HasColumnName(ColumnPrefix + "id")
            .IsRequired();
        builder.Property(p => p.ExtensionType)
            .HasColumnName(ColumnPrefix + "type")
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
        builder.Property(p => p.GitHubRepository)
            .HasColumnName(ColumnPrefix + "github_repository")
            .IsRequired();
        builder.Property(p => p.DSupportUserId)
            .HasColumnName(ColumnPrefix + "discord_author_userid")
            .IsRequired();
        builder.Property(p => p.Downloads)
            .HasColumnName(ColumnPrefix + "downloads")
            .HasDefaultValue(0);

        builder.HasMany(p => p.ExtensionFiles)
            .WithOne(e => e.ExtensionEntity)
            .HasForeignKey(e => e.ExtensionFileId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}