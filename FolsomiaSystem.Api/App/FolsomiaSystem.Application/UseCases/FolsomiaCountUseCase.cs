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
using System.IO;

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

        public  Task<FolsomiaCount> CountFolsomiaCandidaAsync(FolsomiaCountInput folsomiaCountInput, string folsomiaJob, string fileShared)
        {
                var folsomiacount = _mapper.Map<FolsomiaCount>(folsomiaCountInput);
                var idTest = Guid.NewGuid();
                var imageOut = string.Format(@"{0}.jpg", idTest);
                var taskFolsomiaCount = new TaskCompletionSource<FolsomiaCount>();
                folsomiacount.IdTest = idTest.ToString();
                folsomiacount.ImageFolsomiaURL = Path.Combine(fileShared, imageOut);
                folsomiacount.FileResult = new FileToUpload()
                {
                   FileAsBase64 = folsomiaCountInput.FileAsBase64
                };
                
                folsomiacount =  this.UploadImage(folsomiacount);

                if (folsomiacount.AuditLog.StatusLog == StatusLog.fail)
                {
                    taskFolsomiaCount.SetResult(folsomiacount);
                    return taskFolsomiaCount.Task;
                }

                folsomiacount.AuditLog.OperationLog = OperationLog.CountFolsomia;
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
                    folsomiacount.ImageFolsomiaURL = "";
                    
                    _auditLogExternalService.AddNewLogAuditAsync(folsomiacount.AuditLog);
                    _unitOfWork.CommitAsync();
                    return res;
                }
           
        }
        

        private FolsomiaCount UploadImage(FolsomiaCount folsomiaCount)
        {
            try
            {

                if (folsomiaCount.FileResult.FileAsBase64.Contains(","))
                {
                    folsomiaCount.FileResult.FileAsBase64 = folsomiaCount.FileResult.FileAsBase64.Substring(folsomiaCount.FileResult.FileAsBase64.IndexOf(",") + 1);
                }

                folsomiaCount.FileResult.FileAsByteArray = Convert.FromBase64String(folsomiaCount.FileResult.FileAsBase64);


                using (var fs = new FileStream(folsomiaCount.ImageFolsomiaURL, FileMode.CreateNew))
                {
                    fs.Write(folsomiaCount.FileResult.FileAsByteArray, 0, folsomiaCount.FileResult.FileAsByteArray.Length);
                }

                folsomiaCount.AuditLog.MessageLog = "Upload file success.";
                folsomiaCount.AuditLog.StatusLog = StatusLog.success;
                folsomiaCount.AuditLog.OperationLog = OperationLog.UploadFile;

                _auditLogExternalService.AddNewLogAuditAsync(folsomiaCount.AuditLog);
                _unitOfWork.CommitAsync();

                return folsomiaCount;
            }catch(Exception e)
            {
                folsomiaCount.AuditLog.MessageLog = "Upload file failed.";
                folsomiaCount.AuditLog.StatusLog = StatusLog.fail;
                folsomiaCount.AuditLog.OperationLog = OperationLog.UploadFile;
                _auditLogExternalService.AddNewLogAuditAsync(folsomiaCount.AuditLog);
                _unitOfWork.CommitAsync();
                return folsomiaCount;
            }

        } 
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}

            