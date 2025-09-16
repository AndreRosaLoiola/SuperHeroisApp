using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Repositories;

namespace WebApplication1.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddProjectDependencies(this IServiceCollection services)
        {
            // Repositórios
            services.AddScoped<IHeroiRepository, HeroiRepository>();
            // Adicione outras injeções aqui
            return services;
        }
    }
}
