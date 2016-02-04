﻿using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json.Serialization;

namespace MongoMvc
{
    public class Startup
    {
        public Startup(IApplicationEnvironment appEnv)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(appEnv.ApplicationBasePath)
                .AddJsonFile("config.json")
                .AddEnvironmentVariables();

            Configuration = configurationBuilder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddMvc().AddJsonOptions(o => o.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver());

            services.Configure<Settings>(Configuration);
            services.AddSingleton<IArticleRespository, ArticleRepository>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseCors(builder => builder.AllowAnyOrigin());

            // Add the platform handler to the request pipeline.
            app.UseMvc();
            //app.UseWelcomePage();
            //app.UseIISPlatformHandler();

            app.Run(async context => { await context.Response.WriteAsync("Hello World!"); });
        }
    }
}