using CDN.Application.Interfaces;
using CDN.Infrastructure.Data;
using CDN.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CDN.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<AppDbContext>()
            .AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}