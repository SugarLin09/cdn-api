using CDN.Application.Interfaces;
using CDN.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CDN.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}