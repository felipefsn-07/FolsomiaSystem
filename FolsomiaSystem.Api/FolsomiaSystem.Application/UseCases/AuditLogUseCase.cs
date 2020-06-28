using AutoMapper;
using FolsomiaSystem.Application.Interfaces;
using FolsomiaSystem.Application.Interfaces.Repositories;
using FolsomiaSystem.Application.Models;
using FolsomiaSystem.Domain;
using FolsomiaSystem.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace FolsomiaSystem.Application.UseCases
{
    public class AuditLogUseCase : IAuditLogUseCase
    {
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AuditLogUseCase(
            IAuditLogRepository auditLogRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _auditLogRepository = auditLogRepository;

            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AuditLog> AddNewLogAuditAsync(AuditLogInput auditLogInput)
        {
            var auditLog = _mapper.Map<AuditLog>(auditLogInput);
            // var validator = new AuditLogValidator();
            //if (!validator.Validate(auditLog).IsValid) throw new CustomerInvalidException("Customer Invalid");

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
            throw new NotImplementedException();
        }

        /* public Task<IPagedList<AuditLog>> GetAllAsync(AuditLogInput auditLogFilter, PagedListIntput request)
         {
             var auditLog = _mapper.Map<AuditLog>(auditLogFilter);
             return _auditLogRepository.GetAsync(auditLog, request.PageNumber, request.PageSize);

         }*/

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
