using FolsomiaSystem.Application.DTOs;
using FolsomiaSystem.Domain;
using FolsomiaSystem.Domain.Entities;
using System;
using System.Threading.Tasks;


namespace FolsomiaSystem.Application.Interfaces.ExternalServices
{
    public interface IAuditLogExternalService : IDisposable
    {
        Task<AuditLog> AddNewLogAuditAsync(AuditLog auditLog);
        Task<IPagedList<AuditLog>> GetAllAsync(PagedListIntput request);
        Task<IPagedList<AuditLog>> GetAllAsync(AuditLogInput auditLogFilter, PagedListIntput request);
    }
}