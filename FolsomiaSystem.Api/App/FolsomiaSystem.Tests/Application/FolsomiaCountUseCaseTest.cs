using FolsomiaSystem.Domain.Entities;
using FolsomiaSystem.Application.DTOs;
using FolsomiaSystem.Application.UseCases;
using FolsomiaSystem.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using FolsomiaSystem.Application.Interfaces.ExternalServices;
using FolsomiaSystem.Domain;
using AutoMapper;
using Moq;
using FolsomiaSystem.Infra.ExternalServices.FolsomiaCountJob;
using FolsomiaSystem.Infra.ES.Logging;

namespace FolsomiaSystem.Tests.Application
{
    public class FolsomiaCountUseCaseTest
    {
        private readonly IFolsomiaCountUseCase _folsomiaCountUseCase;
        private readonly IFolsomiaCountJob _folsomiaCountJob;
        private readonly IMapper _mapper;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<IAuditLogExternalService> _auditLogExternalService;


        public FolsomiaCountUseCaseTest()
        {
            _mapper = MapperFactory.Create();
            _folsomiaCountJob = new FolsomiaCountJob();
            _auditLogExternalService = new Mock<IAuditLogExternalService>();

            _unitOfWork = new Mock<IUnitOfWork>();

            _folsomiaCountUseCase = new FolsomiaCountUseCase(
                _folsomiaCountJob,
                _mapper,
                _auditLogExternalService.Object,
                _unitOfWork.Object
                );
        }

        #region Constants

        private const string LOCAL_PYTHON_FOLSOMIA_COUNT = "C:/Users/cs319260/Desktop/TCC/Sistema/FolsomiaSystem.Job/App/folsomiacount.py";

        public readonly FolsomiaCountInput folsomiaCountInput = new FolsomiaCountInput()
        {
            IdTest = "1",
            ImageFolsomiaURL = "C:/Users/cs319260/Desktop/TCC/Sistema/FolsomiaSystem.Job/App/test_inputs_outputs/inputs/test_in_1.jpg",
            BackgroundImage = 0,
        };

        #endregion

        [Fact]

        public void CountFolsomiaCandidaValidInformation()
        {
            var folsomia = _folsomiaCountUseCase.CountFolsomiaCandidaAsync(folsomiaCountInput, LOCAL_PYTHON_FOLSOMIA_COUNT);
            Assert.True(folsomia?.Result.TotalCountFolsomia != null);
        }
    }
}
