using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CliClient
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                        .ConfigureServices((hostContext, services) =>
                        {
                            // Register your services here
                            services.AddSingleton<App>();
                        })
                        .Build();

            var app = host.Services.GetRequiredService<App>();
            await app.RunAsync();
        }
    }
}
