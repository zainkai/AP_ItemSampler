using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SmarterBalanced.SampleItems.Core.Diagnostics;
using SmarterBalanced.SampleItems.Core.Repos;
using SmarterBalanced.SampleItems.Dal.Configurations.Models;
using SmarterBalanced.SampleItems.Dal.Providers;
using System;

namespace SmarterBalanced.SampleItems.Web
{
    public class Startup
    {
        private readonly ILogger logger;
        public Startup(IHostingEnvironment env, ILoggerFactory factory)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            if (env.IsDevelopment())
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            Configuration = builder.Build();
            ConfigureLogging(env, factory);

            logger = factory.CreateLogger<Startup>();
        }

        private void ConfigureLogging(IHostingEnvironment env, ILoggerFactory factory)
        {
            factory.AddConsole(Configuration.GetSection("Logging"));
            factory.AddDebug();
            if (!env.IsDevelopment())
            {
                factory.AddAWSProvider(Configuration.GetAWSLoggingConfigSection());
            }
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            SampleItemsContext context;
            AppSettings appSettings = new AppSettings();

            Configuration.Bind(appSettings);
            try
            {
                context = SampleItemsProvider.LoadContext(appSettings, logger).Result;
            }
            catch (Exception e)
            {
                logger.LogCritical($"{e.Message} occured when loading the context");
                throw e;
            }

            services.AddApplicationInsightsTelemetry(Configuration);
            services.AddMvc();
            services.AddRouting();

            services.AddSingleton(context);
            services.AddSingleton(appSettings);
            services.AddScoped<IItemViewRepo, ItemViewRepo>();
            services.AddScoped<ISampleItemsSearchRepo, SampleItemsSearchRepo>();
            services.AddScoped<IDiagnosticManager, DiagnosticManager>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseApplicationInsightsExceptionTelemetry();
            app.UseStaticFiles();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseStatusCodePagesWithRedirects("/Home/StatusCodeError?code={0}");
            }

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "diagnostic",
                    template: "status/{level?}",
                    defaults: new { controller = "Diagnostic", action = "Index" });
            });

        }

    }
}
