using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSE.Core.Util;
using NSE.MessageBus;

namespace NSE.Identity.API.Configuration
{
	public static class MessageBusConfig
	{
		public static void AddMessageBusConfiguration(this IServiceCollection services,
													 IConfiguration configuration)
		{
			services.AddMessageBus(configuration.GetMessageQueueConnection(name: "MessageBus"));
		}
	}
}
