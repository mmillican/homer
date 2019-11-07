using Homer.Shared.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Homer.Shared
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHomerServices(this IServiceCollection services)
        {
            services.AddScoped<IDbContext, HomerDbContext>();
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

            return services;
        }
    }
}