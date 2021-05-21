using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ParentCheck.Data;
using ParentCheck.Web.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web
{
    public class Startup
    {
        private IMediator mediator;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connString = AppHelper.Settings.ConnectionStrings.ParentCheck_Database_Connection_String;
            services.AddMemoryCache();
            services.AddCors();
            services.AddApplicationInsightsTelemetry();

            services.AddEntityFrameworkSqlServer()
                .AddDbContext<ParentcheckContext>(options => options.UseSqlServer(connString), ServiceLifetime.Transient);

            services.AddSingleton<IMediator>(m => mediator);

            
            //services.AddControllers();
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void AssurePathExists(string path)
        {
            var directory = Path.GetDirectoryName(path);

            //If 'directory' is empty that means the path already points to a directory
            if (string.IsNullOrWhiteSpace(directory))
            {
                directory = path;
            }

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }
    }
}
