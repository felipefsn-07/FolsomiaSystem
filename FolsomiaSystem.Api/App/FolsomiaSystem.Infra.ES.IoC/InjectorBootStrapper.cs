using FolsomiaSystem.Application.Interfaces;
using FolsomiaSystem.Application.Interfaces.ExternalServices;
using FolsomiaSystem.Application.Interfaces.Repositories;
using FolsomiaSystem.Application.UseCases;
using FolsomiaSystem.Domain;
using FolsomiaSystem.Infra.Data;
using FolsomiaSystem.Infra.Data.Repositories;
using FolsomiaSystem.Infra.ES.CredentialSafe;
using Microsoft.Extensions.DependencyInjection;
using FolsomiaSystem.Domain.Entities;
using FolsomiaSystem.Infra.Caching;
using FolsomiaSystem.Infra.ExternalServices.FolsomiaCountJob;
using FolsomiaSystem.Infra.ES.Logging;

namespace FolsomiaSystem.Infra.IoC
{
    public static class InjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            // Application
            services.AddScoped<IFolsomiaCountUseCase, FolsomiaCountUseCase>();
            services.AddScoped<IFolsomiaSetupUseCase, FolsomiaSetupUseCase>();

            // Infra - Data
            services.AddScoped<IAuditLogRepository, AuditLogRepository>();
            services.AddScoped<IFolsomiaSetupRepository, FolsomiaSetupRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Infra - Data - Login
            services.AddScoped<IAdminUserRepository, AdminUserRepository>();


            // Infra - Caching (singleton)
            services.AddSingleton<ICache<CredentialSafeConfig>, MemoryCache<CredentialSafeConfig>>();

            // Infra - External Services
            services.AddScoped<IFolsomiaCountJob, FolsomiaCountJob>();
            services.AddScoped<IAuditLogExternalService, AuditLogExternalService>();
            services.AddScoped<ICredentialSafeExternalService, CredentialSafeExternalService>();
        }
    }
}