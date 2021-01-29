using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;

using GameSystemApi.Repositories;
using GameSystemApi.Services;
using GameSystemApi.EventProcessors;

using TbspRpgLib;

namespace GameSystemApi
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
            LibStartup.ConfigureTbspRpgServices(Configuration, services);

            var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

            services.AddDbContext<GameSystemContext>(
                options => options.UseNpgsql(connectionString)
            );

            services.AddScoped<IGameSystemRepository, GameSystemRepository>();
            services.AddScoped<IGameSystemService, GameSystemService>();
            services.AddScoped<IEnterLocationEventHandler, EnterLocationEventHandler>();

            //start workers
            services.AddHostedService<EventProcessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            LibStartup.ConfigureTbspRpg(app);

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
