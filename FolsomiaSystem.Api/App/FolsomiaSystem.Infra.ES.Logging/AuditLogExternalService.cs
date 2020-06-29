using AutoMapper;
using FolsomiaSystem.Application.Interfaces.ExternalServices;
using FolsomiaSystem.Application.Interfaces.Repositories;
using FolsomiaSystem.Application.DTOs;
using FolsomiaSystem.Domain;
using FolsomiaSystem.Domain.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FolsomiaSystem.Infra.ES.Logging
{
    public class AuditLogExternalService : IAuditLogExternalService
    {
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AuditLogExternalService(
            IAuditLogRepository auditLogRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _auditLogRepository = auditLogRepository;

            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AuditLog> AddNewLogAuditAsync(AuditLog auditLog)
        {
            await _auditLogRepository.AddAsync(auditLog);
            await _unitOfWork.CommitAsync();
            return auditLog;
        }

        public Task<IPagedList<AuditLog>> GetAllAsync(PagedListIntput request)
        {
            return _auditLogRepository.GetAsync(p => p.AuditLogId, request.PageNumber, request.PageSize);
        }


        public Task<IPagedList<AuditLog>> GetAllAsync(AuditLogInput auditLogFilter, PagedListIntput request)
        {            

            return _auditLogRepository.getAllAuditLogByFilter(auditLogFilter, request.PageNumber, request.PageSize);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

    }
}
