using FolsomiaSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FolsomiaSystem.Infra.Data.EntityConfigurations
{
    public class FolsomiaSetupConfiguration : IEntityTypeConfiguration<FolsomiaSetup>
    {
        public void Configure(EntityTypeBuilder<FolsomiaSetup> builder)
        {
            builder.ToTable("tb_folsomia_setup");


            builder.Property(p => p.MaxTest)
                .IsRequired();

            builder.Property(p => p.MaxConcentration)
                .IsRequired();

            builder.HasKey(chave => chave.FolsomiaSetupId);
            builder.Property(t => t.FolsomiaSetupId).HasColumnName("FolsomiaSetupId");
        }
    }
}
