using MicroManagement.Persistence.Abstraction.Repositories;
using MicroManagement.Persistence.EF.Configuration;
using MicroManagement.Persistence.EF.Repositories;
using MicroManagement.Services;
using MicroManagement.Services.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace MicroManagement.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var app = Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

            return app;
        }
    }
}