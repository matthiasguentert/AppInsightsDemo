using Azure.Storage.Blobs;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.ApplicationInsights.SnapshotCollector;
using Microsoft.ApplicationInsights.AspNetCore;
using System;
using Microsoft.Extensions.Options;

namespace ServiceApi
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
            // Configure SnapshotCollector from application settings
            services.Configure<SnapshotCollectorConfiguration>(Configuration.GetSection(nameof(SnapshotCollectorConfiguration)));

            // Add SnapshotCollector telemetry processor.
            services.AddSingleton<ITelemetryProcessorFactory>(sp => new SnapshotCollectorTelemetryProcessorFactory(sp));

            // Add Application Insights
            services.AddApplicationInsightsTelemetry();

            // Securing the control channel 
            services.ConfigureTelemetryModule<QuickPulseTelemetryModule>((module, o) => module.AuthenticationApiKey = Configuration["AppInsightsApiKey"]);

            // Register Initializer to set Cloud Role Name 
            services.AddSingleton<ITelemetryInitializer, CloudRoleNameTelemetryInitializer>();

            services.AddControllers();

            // Register azure blob container client
            services.AddSingleton(new BlobContainerClient(Configuration["ConnectionString"], "uploads"));

            // Add App Insights Snapshot Debugger
            services.AddSnapshotCollector((configuration) => Configuration.Bind(nameof(SnapshotCollectorConfiguration), configuration));
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

        private class SnapshotCollectorTelemetryProcessorFactory : ITelemetryProcessorFactory
        {
            private readonly IServiceProvider _serviceProvider;

            public SnapshotCollectorTelemetryProcessorFactory(IServiceProvider serviceProvider) =>
                _serviceProvider = serviceProvider;

            public ITelemetryProcessor Create(ITelemetryProcessor next)
            {
                var snapshotConfigurationOptions = _serviceProvider.GetService<IOptions<SnapshotCollectorConfiguration>>();
                return new SnapshotCollectorTelemetryProcessor(next, configuration: snapshotConfigurationOptions.Value);
            }
        }
    }
}
