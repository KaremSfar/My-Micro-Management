using CliClient.States;
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
                            services.AddSingleton<App>();
                            services.AddSingleton<IAuthService>(c =>
                            {
                                return new AuthService("https://localhost:44325", c.GetService<TokenStorage>()!);
                            });
                            services.AddSingleton<TokenStorage>();
                            services.AddTransient<LoginState>();
                            services.AddTransient<StateProvider>();
                        })
                        .Build();

            var app = host.Services.GetRequiredService<App>();
            await app.RunAsync();
        }
    }
}
