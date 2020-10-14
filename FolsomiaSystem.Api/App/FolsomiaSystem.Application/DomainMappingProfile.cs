using AutoMapper;
using FolsomiaSystem.Application.DTOs;
using FolsomiaSystem.Domain.Entities;
using FolsomiaSystem.Domain.Enums;
using System;

namespace FolsomiaSystem.Application
{
    public class DomainMappingProfile : Profile
    {
        public DomainMappingProfile()
        {
            DomainToDto();
            DtoToDomain();
        }

        public void DomainToDto()
        {
            CreateMap<FolsomiaCount, FolsomiaCountInput>()
          .ForMember(p => p.IdTest, p => p.MapFrom(m => new FolsomiaCountInput
          {
              IdTest = m.IdTest,
              BackgroundImage = m.BackgroundImage

          }));

            CreateMap<FolsomiaSetup, FolsomiaSetupInput>();
            CreateMap<AuditLog, AuditLogInput>();
            CreateMap<FolsomiaCount, AuditLogInput>();
            CreateMap<FolsomiaSetup, AuditLogInput>();



        }
        public void DtoToDomain()
        {
            CreateMap<FolsomiaSetupInput, FolsomiaSetup>()
                .ForMember(p => p.FolsomiaSetupId, p => p.MapFrom(m => new FolsomiaSetup
                {
                    MaxConcentration = m.MaxConcentration,
                    MaxTest = m.MaxTest
                }));


            CreateMap<FolsomiaCountInput, FolsomiaCount>()
            .ForMember(p => p.IdTest, p => p.MapFrom(m => new FolsomiaCount
            {
                IdTest = m.IdTest,
                BackgroundImage = m.BackgroundImage

            }));

            CreateMap<AuditLogInput, AuditLog>();
        }
    }
}
