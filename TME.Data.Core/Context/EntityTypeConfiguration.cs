
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TME.Domain.Core.Entities;


namespace TME.Data.Core.Context
{
    public abstract class EntityTypeConfiguration<TEntity, TKey>
        where TEntity : EntityBase<TEntity, Guid>, IEntity<Guid>
        where TKey : struct
    {
        protected void ConfigureBasicFields(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(field => field.Id)
                .ValueGeneratedNever()
                .HasColumnType("uniqueidentifier");
            builder.HasKey(field => field.Id);

            builder.Property(field => field.CreatedByApplicationUserId)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            builder.Property(field => field.CreatedOn)
                .IsRequired()
                .HasColumnType("datetime");

            builder.Property(field => field.LastUpdatedByApplicationUserId)
                .HasColumnType("uniqueidentifier");

            builder.Property(field => field.LastUpdated)
                .HasColumnType("datetime");

            builder.Property(field => field.IsActive)
                .HasColumnType("bit");

            builder.Property(field => field.IsDeleted)
                .HasColumnType("bit");
        }
    }
}
