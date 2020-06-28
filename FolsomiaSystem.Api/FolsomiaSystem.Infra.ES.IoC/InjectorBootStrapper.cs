using FolsomiaSystem.Application.Interfaces;
using FolsomiaSystem.Application.Interfaces.ExternalServices;
using FolsomiaSystem.Application.Interfaces.Providers;
using FolsomiaSystem.Application.Interfaces.Repositories;
using FolsomiaSystem.Application.UseCases;
using FolsomiaSystem.Domain;
using FolsomiaSystem.Infra.Data;
using FolsomiaSystem.Infra.Data.Providers;
using FolsomiaSystem.Infra.Data.Repositories;
using FolsomiaSystem.Infra.ES.CredentialSafe;
using Microsoft.Extensions.DependencyInjection;
using FolsomiaSystem.Domain.Entities;
using FolsomiaSystem.Infra.Caching;
using FolsomiaSystem.Infra.ExternalServices.FolsomiaCountJob;

namespace FolsomiaSystem.Infra.IoC
{
    public static class InjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            // Application
            services.AddScoped<IFolsomiaCountUseCase, FolsomiaCountUseCase>();
            services.AddScoped<IAuditLogUseCase, AuditLogUseCase>();
            services.AddScoped<IFolsomiaSetupUseCase, FolsomiaSetupUseCase>();

            // Infra - Data
            services.AddScoped<IAuditLogRepository, AuditLogRepository>();
            services.AddScoped<IFolsomiaSetupRepository, FolsomiaSetupRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Infra - Data - Cofre de Senhas (singleton)
            //services.AddSingleton<ICredentialSafeRepository, CredentialSafeRepository>();
            //services.AddSingleton<ICredentialSafeConfigProvider, CredentialSafeConfigProvider>();

            // Infra - Data - Provedor de strings de conexão SQL Server (singleton)
            //services.AddSingleton<ISqlConnectionStringProvider, SqlConnectionStringProvider>();

            // Infra - Caching (singleton)
            //services.AddSingleton<ICache<Credential>, MemoryCache<Credential>>();

            // Infra - External Services
            services.AddScoped<IFolsomiaCountJob, FolsomiaCountJob>();
           // services.AddSingleton<ICredentialSafeExternalService, CredentialSafeExternalService>();
        }
    }
}