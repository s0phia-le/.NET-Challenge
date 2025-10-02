using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using CodeChallenge.Data;
using CodeChallenge.Repositories;
using CodeChallenge.Services;
using CodeChallenge.Helpers;
using Microsoft.Extensions.Hosting;

namespace CodeChallenge
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // Configures services and request pipelines
        public void ConfigureServices(IServiceCollection services)
        {
            // Configure in-memory database for employee data
            services.AddDbContext<EmployeeContext>(options =>
                options.UseInMemoryDatabase("EmployeeDB"));

            // Register repositories for dependency injections
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<ICompensationRepository, CompensationRepository>();

            // Populate DB on startup
            services.AddTransient<EmployeeDataSeeder>();

            // Mapper to handle DTO-to-Entity conversions
            services.AddScoped<IMapper, Mapper>(); 

            // Register services for business logic
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IReportingStructureService, ReportingStructureService>();
            services.AddScoped<ICompensationService, CompensationService>();

            // Disable endpoint routing to allow UseMvc
            services.AddMvc(options => options.EnableEndpointRouting = false);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, EmployeeDataSeeder seeder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                seeder.Seed().Wait();
            }
            // use MVC routing
            app.UseMvc();
        }
    }
}
