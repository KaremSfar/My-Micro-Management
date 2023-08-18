
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
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddTransient<IProjectsRepository, SQLiteProjectsRepository>();
            builder.Services.AddTransient<IProjectsService, ProjectsService>();

            // TODO-KAREM: this is now here as we're option for a Local SQLite DB, 
            // when, and if, we're getting this project to a client-service app, remove these
            builder.Services.AddDbContext<MyMicroManagementDbContext>(options =>
            {
                options.UseSqlite(@"Data Source=C:\Repos\Temp\MyDB-dev.db");
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}