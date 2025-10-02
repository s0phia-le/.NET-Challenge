using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using CodeChallenge.Data;
using Microsoft.EntityFrameworkCore;
using CodeChallenge.Repositories;
using CodeChallenge.Services;
using CodeChallenge.Helpers;
using challenge.Services;

namespace code_challenge.Tests.Integration
{
    public class TestServerStartup
    {
        public TestServerStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<EmployeeContext>(options =>
            {
                options.UseInMemoryDatabase("EmployeeDB");
            });
            services.AddScoped<IEmployeeRepository, EmployeeRespository>();
            services.AddTransient<EmployeeDataSeeder>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IReportingStructureService, ReportingStructureService>();
            services.AddScoped<IMapper, Mapper>();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, EmployeeDataSeeder seeder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                seeder.Seed().Wait();
            }

            app.UseMvc();

        }
    }
}