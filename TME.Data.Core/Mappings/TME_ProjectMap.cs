using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TME.Data.Core.Context;
using TME.Domain.Core.Entities;


namespace TME.Data.Core.Mappings
{
    internal class TME_ProjectMap: EntityTypeConfiguration<TME_Project, Guid>,
        IEntityTypeConfiguration<TME_Project>
    {
        public void Configure(EntityTypeBuilder<TME_Project> builder)
        {
            builder
                .ToTable("TME_PROJECT");

            builder
                .HasKey(field => field.Id);

            builder
                .Property(field => field.Description)
                .HasColumnType("varchar(1024)")
                .HasMaxLength(1024)
                .IsRequired();

            builder
                .HasMany(sm => sm.Tasks)
                .WithOne(slof => slof.Project)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            base.ConfigureBasicFields(builder);
        }
    }
}
