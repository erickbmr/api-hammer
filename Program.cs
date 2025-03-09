using Lotest.Clients;
using Lotest.Clients.Interfaces;
using Lotest.Worker;
using Microsoft.Extensions.DependencyInjection;

namespace Lotest
{
    public class Program
    {
        static void Main(string[] args)
        {
            var servicesCollection = new ServiceCollection();
            ConfigureServices(servicesCollection);
            var serviceProvider = servicesCollection.BuildServiceProvider();
            InputWorker.Start(serviceProvider);
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            services
                .AddHttpClient<IClient, ClientHttp>()
                .SetHandlerLifetime(TimeSpan.FromMinutes(10));
        }
    }
}
