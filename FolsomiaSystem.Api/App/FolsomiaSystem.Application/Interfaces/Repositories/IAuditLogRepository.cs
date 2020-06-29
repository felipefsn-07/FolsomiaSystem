using FolsomiaSystem.Application.DTOs;
using FolsomiaSystem.Domain;
using FolsomiaSystem.Domain.Entities;
using System.Threading.Tasks;

namespace FolsomiaSystem.Application.Interfaces.Repositories
{
    public interface IAuditLogRepository : IRepository<AuditLog>
    {
        Task<IPagedList<AuditLog>> getAllAuditLogByFilter(AuditLogInput filter, int pageNumber, int pageSize);
    }
}
