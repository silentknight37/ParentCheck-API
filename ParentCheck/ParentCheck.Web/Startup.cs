using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ParentCheck.Data;
using ParentCheck.Envelope;
using ParentCheck.Web.Common;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

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
            services.AddMvc();
            services.AddCors();
            services.AddApplicationInsightsTelemetry();

            services.AddEntityFrameworkSqlServer()
                .AddDbContext<ParentCheckContext>(options => options.UseSqlServer(connString), ServiceLifetime.Transient);

            services.AddMediatR(typeof(PackageEnvelop).Assembly);

            services.AddSingleton<IMediator>(m => mediator);

            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);
            services.AddMvc(AddMvcFilters)
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                    options.JsonSerializerOptions.DictionaryKeyPolicy = null;
                });
            services.AddSwaggerGen(x=>
            {
                x.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo{ Title = "Parent Check API", Version = "v1" });
                x.CustomSchemaIds(y => y.FullName);
                x.DocInclusionPredicate((version, apiDescription) => true);
                x.TagActionsBy(y => y.GroupName);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger(c =>
            {
                c.RouteTemplate = "swagger/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "Parent Check API V1");
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //app.UseAuthorization();
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

        private void AddMvcFilters(Microsoft.AspNetCore.Mvc.MvcOptions mvcOptions)
        {
            GetMvcFilters().ForEach(f => mvcOptions.Filters.Add(f));
        }
        private List<IFilterMetadata> GetMvcFilters()
        {
            var filterCollection = new List<IFilterMetadata>();
            filterCollection.Add(new GlobalExceptionFilter());

            return filterCollection;
        }
    }
}
