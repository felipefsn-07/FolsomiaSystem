using AutoMapper;
using FolsomiaSystem.Application.Interfaces;
using FolsomiaSystem.Application.Interfaces.Repositories;
using FolsomiaSystem.Application.DTOs;
using FolsomiaSystem.Application.Validators;
using FolsomiaSystem.Domain;
using FolsomiaSystem.Domain.Entities;
using System;
using System.Threading.Tasks;
using FolsomiaSystem.Domain.Enums;
using FolsomiaSystem.Application.Interfaces.ExternalServices;

namespace FolsomiaSystem.Application.UseCases
{
    public class FolsomiaSetupUseCase : IFolsomiaSetupUseCase
    {
        private readonly IAuditLogExternalService _auditLogExternalService;
        private readonly IFolsomiaSetupRepository _folsomiaSetupRepository;
        private readonly IUnitOfWork _unitOfWork;

        public FolsomiaSetupUseCase(
            IFolsomiaSetupRepository folsomiaSetupRepository,
            IAuditLogExternalService auditLogExternalService,
            IUnitOfWork unitOfWork)
        {
            _folsomiaSetupRepository = folsomiaSetupRepository;
            _unitOfWork = unitOfWork;
            _auditLogExternalService = auditLogExternalService;
        }



        public Task<FolsomiaSetup> GetAsync()
        {
            var res =  _folsomiaSetupRepository.GetFirstAsync();
             _unitOfWork.CommitAsync();
            return res;
        }

        public async Task UpdateSetup(FolsomiaSetupInput folsomiaSetupInput) {

            var folsomiaSetup = await _folsomiaSetupRepository.GetFirstAsync();
            folsomiaSetup.MaxConcentration = folsomiaSetupInput.MaxConcentration;
            folsomiaSetup.MaxTest = folsomiaSetupInput.MaxTest;
            folsomiaSetup.AuditLog.OperationLog = OperationLog.CountFolsomia;
            var taskFolsomiaSetup = new TaskCompletionSource<FolsomiaSetup>();
            var validator = new FolsomiaSetupValidator();
            BaseValidator baseValidator = new BaseValidator();
            var validRes = validator.Validate(folsomiaSetup);
            if (!validator.Validate(folsomiaSetup).IsValid)
            {
                string msg = baseValidator.MsgErrorValidator(validRes);
                folsomiaSetup.AuditLog.StatusLog = StatusLog.fail;
                folsomiaSetup.AuditLog.MessageLog = msg;
                taskFolsomiaSetup.SetResult(folsomiaSetup);
                await _auditLogExternalService.AddNewLogAuditAsync(folsomiaSetup.AuditLog);
                await _unitOfWork.CommitAsync();
                await taskFolsomiaSetup.Task;
            }
            else
            {
                 _folsomiaSetupRepository.Update(folsomiaSetup);
                await _auditLogExternalService.AddNewLogAuditAsync(folsomiaSetup.AuditLog);
                await _unitOfWork.CommitAsync();
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }


    }
}