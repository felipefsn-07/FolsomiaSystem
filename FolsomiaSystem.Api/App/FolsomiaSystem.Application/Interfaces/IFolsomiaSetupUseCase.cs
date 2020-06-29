using FolsomiaSystem.Application.DTOs;
using FolsomiaSystem.Domain;
using FolsomiaSystem.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace FolsomiaSystem.Application.Interfaces
{
    public interface IFolsomiaSetupUseCase : IDisposable
    {
        Task UpdateSetup(FolsomiaSetupInput folsomiaSetupInput);
        Task<FolsomiaSetup> GetAsync();
    }
}