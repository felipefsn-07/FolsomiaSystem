using FolsomiaSystem.Application.Models;
using FolsomiaSystem.Domain;
using FolsomiaSystem.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace FolsomiaSystem.Application.Interfaces
{
    public interface IAuditLogUseCase : IDisposable
    {

        Task<AuditLog> AddNewLogAuditAsync(AuditLogInput auditLogInput);
        Task<IPagedList<AuditLog>> GetAllAsync(PagedListIntput request);
        Task<IPagedList<AuditLog>> GetAllAsync(AuditLogInput auditLogFilter, PagedListIntput request);

    }
}