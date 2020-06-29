using FolsomiaSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FolsomiaSystem.Infra.Data.EntityConfigurations
{
    public class AdminUserConfiguration : IEntityTypeConfiguration<AdminUser>
    {
        public void Configure(EntityTypeBuilder<AdminUser> builder)
        {

            builder.ToTable("tb_user_admin");

            builder.Property(t => t.UserName).HasColumnName("UserName");
            builder.Property(t => t.Password).HasColumnName("Password");
            builder.HasKey(chave => chave.AdminUserId);
            builder.Property(t => t.AdminUserId).HasColumnName("AdminUserId");
        }
    }
}
