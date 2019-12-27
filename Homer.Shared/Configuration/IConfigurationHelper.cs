using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Homer.Shared.Configuration
{
    public interface IConfigurationHelper
    {
        string GetValue(string key, string keyPrefix = null);
    }
    
    public class ConfigurationHelper : IConfigurationHelper
    {
        private readonly IHostEnvironment _environment;
        private readonly IConfiguration _configuration;

        public ConfigurationHelper(IHostEnvironment environment,
            IConfiguration configuration)
        {
            this._environment = environment;
            this._configuration = configuration;
        }

        public string GetValue(string key, string keyPrefix = null)
        {
            if (_environment.IsDevelopment())
            {
                if (!string.IsNullOrEmpty(keyPrefix))
                {
                    key = $"{keyPrefix}:{key}";
                }
                return _configuration.GetValue<string>(key);
            }

            return Environment.GetEnvironmentVariable(key);
        }
    }
}