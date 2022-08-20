using Microsoft.Extensions.Configuration;

namespace NSE.Core.Util
{
    public static class ConfigurationExtensions
    {
        public static string GetMessageQueueConnection(this IConfiguration configuration, string name)
        {
            return configuration?.GetSection(key:"MessageQueueConnection")?[name];
        }
    }
}