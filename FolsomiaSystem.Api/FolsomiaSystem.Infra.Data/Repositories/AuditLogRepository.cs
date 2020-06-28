using FolsomiaSystem.Application.Interfaces.Repositories;
using FolsomiaSystem.Domain.Entities;

namespace FolsomiaSystem.Infra.Data.Repositories
{
    public class AuditLogRepository : RepositoryBase<AuditLog>, IAuditLogRepository
    {
        public AuditLogRepository(DefaultContext context) : base(context)
        {
        }
    }
}
