using System;
using CodeChallenge.Services;
using CodeChallenge.Data;
using CodeChallenge.Repositories;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CodeChallenge.Helpers;

namespace CodeChallenge.Config
{
    public class App
    {
        public WebApplication Configure(string[] args)
        {
            args ??= Array.Empty<string>();

            var builder = WebApplication.CreateBuilder(args);

            builder.UseEmployeeDB();
            
            AddServices(builder.Services);

            var app = builder.Build();

            var env = builder.Environment;
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                SeedEmployeeDB();
            }

            app.UseAuthorization();

            app.MapControllers();

            return app;
        }

        private void AddServices(IServiceCollection services)
        {

            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IMapper, Mapper>();
            services.AddScoped<ICompensationService, CompensationService>();

            services.AddScoped<ICompensationRepository, CompensationRepository>();

            services.AddControllers();
        }

        private void SeedEmployeeDB()
        {
            new EmployeeDataSeeder(
                new EmployeeContext(
                    new DbContextOptionsBuilder<EmployeeContext>().UseInMemoryDatabase("EmployeeDB").Options
            )).Seed().Wait();
        }
    }
}
