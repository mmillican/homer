using Homer.Shared.Configuration;
using Homer.Shared.Data;
using Homer.Shared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Homer.Shared
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHomerServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDefaultAWSOptions(configuration.GetAWSOptions());
            services.AddAWSService<Amazon.DynamoDBv2.IAmazonDynamoDB>();
            
            services.AddSingleton<IConfigurationHelper, ConfigurationHelper>();
            services.AddTransient<IDataContext, DynamoDbContext>();

            services.AddScoped<IDbContext, HomerDbContext>();
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

            return services;
        }
    }
}