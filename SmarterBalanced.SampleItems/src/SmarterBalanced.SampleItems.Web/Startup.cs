using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SmarterBalanced.SampleItems.Core.Diagnostics;
using SmarterBalanced.SampleItems.Core.Repos;
using SmarterBalanced.SampleItems.Dal.Configurations.Models;
using SmarterBalanced.SampleItems.Dal.Providers;

namespace SmarterBalanced.SampleItems.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
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
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry(Configuration);
            services.AddMvc();
            var appSettings = new AppSettings(Configuration); 
            //Build configuration from appsettings.json
            SampleItemsContext.RegisterSettings(appSettings);
            SampleItemsContext.UpdateContent(appSettings);
            // Injecting Singleton SampleItemsRepo into each Controller Repository
            services.AddScoped<IItemViewRepo>(provider => new ItemViewRepo(SampleItemsContext.Default));

            services.AddScoped<ISampleItemsSearchRepo>(provider => new SampleItemsSearchRepo(SampleItemsContext.Default));

            services.AddScoped<IDiagnosticManager>(provider => new DiagnosticManager(SampleItemsContext.Default));

            services.AddRouting();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "diagnostic",
                    template: "status/{action}",
                    defaults: new { controller = "Diagnostic", action = "Index" });
            });

        }

    }

}
