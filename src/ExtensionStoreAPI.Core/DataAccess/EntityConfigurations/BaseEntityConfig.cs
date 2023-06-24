using ExtensionStoreAPI.Core.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExtensionStoreAPI.Core.DataAccess.EntityConfigurations;

public class BaseEntityConfig<T> : IEntityTypeConfiguration<T>
    where T : BaseEntity
{
    protected const string Schema = "extensionstore";
    public required string Table { get; init; }
    public required string ColumnPrefix { get; init; }
    
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .HasColumnName(ColumnPrefix + "id");
    }
}