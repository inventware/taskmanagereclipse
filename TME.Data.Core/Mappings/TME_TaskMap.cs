using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TME.Data.Core.Context;
using TME.Domain.Core.Entities;


namespace TME.Data.Core.Mappings
{
    internal class TME_TaskMap : EntityTypeConfiguration<TME_Task, Guid>,
        IEntityTypeConfiguration<TME_Task>
    {
        public void Configure(EntityTypeBuilder<TME_Task> builder)
        {
            builder
                .ToTable("TME_TASK");

            builder
                .HasKey(field => field.Id);

            builder
                .Property(field => field.Title)
                .HasColumnType("varchar(120)")
                .HasMaxLength(120)
                .IsRequired();

            builder
                .Property(field => field.Description)
                .HasColumnType("varchar(1024)")
                .HasMaxLength(1024)
                .IsRequired();

            builder
                .HasOne(slof => slof.Project)
                .WithMany(sm => sm.Tasks)
                .HasForeignKey(fk => fk.ProjectId)
                .IsRequired();

            base.ConfigureBasicFields(builder);
        }
    }
}
