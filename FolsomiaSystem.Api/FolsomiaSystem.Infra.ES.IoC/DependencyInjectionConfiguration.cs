using Microsoft.Extensions.DependencyInjection;

namespace FolsomiaSystem.Infra.IoC
{
    public static class DependencyInjectionConfiguration
    {
        public static void AddDIConfiguration(this IServiceCollection services)
        {
            InjectorBootStrapper.RegisterServices(services);
        }
    }
}
