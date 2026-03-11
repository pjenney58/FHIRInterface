using Microsoft.Extensions.Configuration;

namespace Support.Model
{
    public static class AppConfig
    {
        public static IConfiguration Get(string configname)
        {
            var config = new ConfigurationBuilder()
               .AddJsonFile(configname)
               .Build();

            return config;
        }
    }
}