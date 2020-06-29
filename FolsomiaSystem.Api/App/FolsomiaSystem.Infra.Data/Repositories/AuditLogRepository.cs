using FolsomiaSystem.Application.DTOs;
using FolsomiaSystem.Application.Interfaces.Repositories;
using FolsomiaSystem.Domain;
using FolsomiaSystem.Domain.Entities;
using FolsomiaSystem.Infra.Data.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FolsomiaSystem.Infra.Data.Repositories
{
    public class AuditLogRepository : RepositoryBase<AuditLog>, IAuditLogRepository
    {
        public AuditLogRepository(DefaultContext context) : base(context)
        {


        }


        public Task<IPagedList<AuditLog>> getAllAuditLogByFilter(AuditLogInput filter, int pageNumber, int pageSize)
        {
            if (filter != null)
            {
                if (filter.DateLogInicio != null) Query.Where(p => p.DateLog>=filter.DateLogInicio);
                if (filter.DateLogFim != null) Query.Where(p => p.DateLog <= filter.DateLogFim);
                if (filter.MessageLog != null) Query.Where(p => p.MessageLog.Contains(filter.MessageLog));
                if (filter.OperationLog != null) Query.Where(p => p.OperationLog == filter.OperationLog);
                if (filter.OperationLog != null) Query.Where(p => p.StatusLog == filter.StatusLog);


            }


            return Query
                .OrderBy(p => p.AuditLogId)
                .ToPagedListAsync(pageNumber, pageSize);

        }
    }
}
