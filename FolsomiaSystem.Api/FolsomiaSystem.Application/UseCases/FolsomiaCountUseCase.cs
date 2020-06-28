using AutoMapper;
using FolsomiaSystem.Application.Interfaces;
using FolsomiaSystem.Application.Interfaces.Repositories;
using FolsomiaSystem.Application.Interfaces.ExternalServices;
using FolsomiaSystem.Application.Models;
using FolsomiaSystem.Domain;
using FolsomiaSystem.Domain.Entities;
using System;
using System.Threading.Tasks;
using FolsomiaSystem.Application.Validators;
using FolsomiaSystem.Application.Exceptions;
using FluentValidation;
using FolsomiaSystem.Domain.Enums;

namespace FolsomiaSystem.Application.UseCases
{

    public class FolsomiaCountUseCase : IFolsomiaCountUseCase
    {
        private readonly IFolsomiaCountJob _folsomiaCountJob;
        private readonly IMapper _mapper;
        private readonly IAuditLogUseCase _auditLogUseCase;


        public FolsomiaCountUseCase(IFolsomiaCountJob folsomiaCountJob,
        IMapper mapper, IAuditLogUseCase auditLogUseCase)
            {
            _folsomiaCountJob = folsomiaCountJob;
                _mapper = mapper;
                _auditLogUseCase = auditLogUseCase;
            }


        public Task<FolsomiaCount> CountFolsomiaCandidaAsync(FolsomiaCountInput folsomiaCountInput, string folsomiaJob)
        {
                var folsomiacount = _mapper.Map<FolsomiaCount>(folsomiaCountInput);
                var taskFolsomiaCount = new TaskCompletionSource<FolsomiaCount>();
                //var auditLog = _mapper.Map<AuditLogInput>(folsomiacount);
                var validator = new FolsomiaCountValidator();
                BaseValidator baseValidator = new BaseValidator();
                var validRes = validator.Validate(folsomiacount);
                if (!validator.Validate(folsomiacount).IsValid)
                {
                    
                    string msg = baseValidator.MsgErrorValidator(validRes);
                    folsomiacount.StatusLog = StatusLog.fail;
                    folsomiacount.MessageLog = msg;
                    taskFolsomiaCount.SetResult(folsomiacount);
                    //_auditLogUseCase.AddNewLogAuditAsync(auditLog);
                    return taskFolsomiaCount.Task;
                }
                return _folsomiaCountJob.FolsomiaCountJobPython(folsomiacount.ImageFolsomiaURL, folsomiacount.ImageFolsomiaOutlinedURL, folsomiaJob);       
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}

            