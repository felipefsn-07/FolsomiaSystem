using FolsomiaSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FolsomiaSystem.Infra.Data.EntityConfigurations
{
    public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.ToTable("tb_audit_log");


            builder.Property(p => p.MessageLog);

            builder.Property(p => p.DateLog)
                .IsRequired();

            builder.Property(p => p.OperationLog)
                .IsRequired();

            builder.Property(p => p.StatusLog)
                .IsRequired();

            builder.HasKey(chave => chave.AuditLogId);
            builder.Property(t => t.AuditLogId).HasColumnName("AuditLogId");
        }
    }
}
