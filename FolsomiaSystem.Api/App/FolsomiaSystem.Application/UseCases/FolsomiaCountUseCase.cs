using AutoMapper;
using FolsomiaSystem.Application.Interfaces;
using FolsomiaSystem.Application.Interfaces.ExternalServices;
using FolsomiaSystem.Application.DTOs;
using FolsomiaSystem.Domain;
using FolsomiaSystem.Domain.Entities;
using System;
using System.Threading.Tasks;
using FolsomiaSystem.Application.Validators;

using FolsomiaSystem.Domain.Enums;

namespace FolsomiaSystem.Application.UseCases
{

    public class FolsomiaCountUseCase : IFolsomiaCountUseCase
    {
        private readonly IFolsomiaCountJob _folsomiaCountJob;
        private readonly IMapper _mapper;
        private readonly IAuditLogExternalService _auditLogExternalService;
        private readonly IUnitOfWork _unitOfWork;
       


        public FolsomiaCountUseCase(IFolsomiaCountJob folsomiaCountJob,
                                    IMapper mapper, 
                                    IAuditLogExternalService auditLogExternalService,
                                    IUnitOfWork unitOfWork
                                    )
        {
            _folsomiaCountJob = folsomiaCountJob;
                _mapper = mapper;
            _auditLogExternalService = auditLogExternalService;
            _unitOfWork = unitOfWork;
        }

        public  Task<FolsomiaCount> CountFolsomiaCandidaAsync(FolsomiaCountInput folsomiaCountInput, string folsomiaJob)
        {
                var folsomiacount = _mapper.Map<FolsomiaCount>(folsomiaCountInput);
                folsomiacount.AuditLog.OperationLog = OperationLog.CountFolsomia;
                var taskFolsomiaCount = new TaskCompletionSource<FolsomiaCount>();
                var validator = new FolsomiaCountValidator();
                BaseValidator baseValidator = new BaseValidator();
                var validRes = validator.Validate(folsomiacount);
                if (!validator.Validate(folsomiacount).IsValid)
                {
                    folsomiacount.AuditLog.StatusLog = StatusLog.fail;
                    folsomiacount.AuditLog.MessageLog = baseValidator.MsgErrorValidator(validRes);
                    taskFolsomiaCount.SetResult(folsomiacount);
                    _auditLogExternalService.AddNewLogAuditAsync(folsomiacount.AuditLog);
                    _unitOfWork.CommitAsync();
                    return taskFolsomiaCount.Task;
            }
            else
            {
                var res = _folsomiaCountJob.FolsomiaCountJobPython(folsomiacount, folsomiaJob);
                _auditLogExternalService.AddNewLogAuditAsync(folsomiacount.AuditLog);
                _unitOfWork.CommitAsync();
                return res;
            }
        }
        
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}

            