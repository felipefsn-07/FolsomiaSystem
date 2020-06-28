using AutoMapper;
using FolsomiaSystem.Application.Interfaces;
using FolsomiaSystem.Application.Interfaces.Repositories;
using FolsomiaSystem.Application.Models;
using FolsomiaSystem.Application.Validators;
using FolsomiaSystem.Domain;
using FolsomiaSystem.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace FolsomiaSystem.Application.UseCases
{
    public class FolsomiaSetupUseCase : IFolsomiaSetupUseCase
    {

        private readonly IFolsomiaSetupRepository _folsomiaSetupRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FolsomiaSetupUseCase(
            IFolsomiaSetupRepository folsomiaSetupRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _folsomiaSetupRepository = folsomiaSetupRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }



        public Task<FolsomiaSetup> GetAsync()
        {
            var res =  _folsomiaSetupRepository.GetFristAsync();
             _unitOfWork.CommitAsync();
            return res;
        }

        public async Task UpdateSetup(FolsomiaSetupInput folsomiaSetupInput) {
            var folsomiaSetup = await _folsomiaSetupRepository.GetFristAsync();
            //if (!await ) throw new CustomerInvalidException("Customer do not eligible to upgrade to VIP");
            var validator = new FolsomiaSetupValidator();
            BaseValidator baseValidator = new BaseValidator();
            var validRes = validator.Validate(folsomiaSetup);
            if (!validator.Validate(folsomiaSetup).IsValid)
            {
                string msg = baseValidator.MsgErrorValidator(validRes);
                throw new NotImplementedException();

            }

            _folsomiaSetupRepository.Update(folsomiaSetup);
            await _unitOfWork.CommitAsync();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }


    }
}