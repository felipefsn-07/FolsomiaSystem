using AutoMapper;
using FolsomiaSystem.Application.Models;
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
              ImageFolsomiaOutlinedURL = m.ImageFolsomiaOutlinedURL,
              ImageFolsomiaURL = m.ImageFolsomiaURL,
              BackgroundImage = m.BackgroundImage

          }));


            CreateMap<FolsomiaSetup, FolsomiaSetupInput>();
           // CreateMap<AdminUserInput, AdminUser>();
            CreateMap<AuditLog, AuditLogInput>();
            CreateMap<FolsomiaCount, AuditLogInput>();


        }
        public void DtoToDomain()
        {
            CreateMap<FolsomiaSetupInput, FolsomiaSetup>();

            CreateMap<FolsomiaCountInput, FolsomiaCount>()
            .ForMember(p => p.IdTest, p => p.MapFrom(m => new FolsomiaCount
            {
                IdTest = m.IdTest,
                ImageFolsomiaOutlinedURL = m.ImageFolsomiaOutlinedURL,
                ImageFolsomiaURL = m.ImageFolsomiaURL,
                BackgroundImage = m.BackgroundImage,
                DateLog = DateTime.Now,
                OperationLog = OperationLog.CountFolsomia,
                StatusLog = StatusLog.success

            }));


            //CreateMap<AdminUserInput, AdminUser>();
            CreateMap<AuditLogInput, AuditLog>();
        }
    }
}
