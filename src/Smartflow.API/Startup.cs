using System;
using System.IO;
using log4net;
using log4net.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NHibernate.NetCore;
using Smartflow.Common;
using Smartflow.API.Code;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Smartflow.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            GlobalObjectService.Configuration = Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // 添加Swagger
            services.AddSwaggerGen(c =>
            {
                //全类型注册
                c.CustomSchemaIds(type => type.FullName);
                
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Smartflow.API", Version = "v1" });
                // 获取xml文件名
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                // 获取xml文件路径
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                // 添加控制器层注释，true表示显示控制器注释
                c.IncludeXmlComments(xmlPath, true);
            });

            services.AddControllers()
                .AddNewtonsoftJson(option =>
            {
                option.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
                option.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                option.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });

            Ioc.RegisterService(services);
            WorkflowGlobalService.RegisterService();
            XmlConfigurator.Configure(LogManager.CreateRepository(GlobalObjectService.Configuration.GetSection("Logging:Program").Value), new FileInfo("log4net.config"));
            services.AddHibernate(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "hibernate.cfg.xml"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors((cors) =>
            {
                cors.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.)
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Smartflow.API");
            });
            
            app.UseRouting();
            app.UseStaticFiles();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
