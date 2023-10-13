using CDN.Application;
using CDN.Application.Interfaces;
using CDN.Application.Services;
using CDN.Infrastructure;
using CDN.Infrastructure.Data;
using CDN.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace CDN.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddRouting(options => options.LowercaseUrls = true)
            .AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo 
                { 
                    Title = "CDN API", 
                    Version = "v1",
                    Description = "API for Complete Developer Network",
                });
            })
            .AddApplication()
            .AddInfrastructure();

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection()
            .UseSwagger();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CDN API V1");
                    c.RoutePrefix = "swagger";
                });
            }
            else
            {
                app.UseHsts();
            }

            app.UseRouting()
            .UseAuthorization()
            .UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}