using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
using Prometheus;
using Prometheus.DotNetRuntime;

namespace TetsWebApp
{
    public class Startup
    {

        private static Options _options;
        public static IDisposable Collector;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            _options = new Options();
            configuration.Bind("Example", _options);

            if (_options.EnableMetrics)
            {
                Collector = CreateCollector();
            }
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
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


        public static IDisposable CreateCollector()
        {
            //_logger.LogInformation($"Configuring prometheus-net.DotNetRuntime: will recycle event listeners every {_options.RecycleEvery} ({_options.RecycleEvery.TotalSeconds:N0} seconds).");

            var builder = DotNetRuntimeStatsBuilder.Default();

            if (!_options.UseDefaultMetrics)
            {
                builder = DotNetRuntimeStatsBuilder.Customize()
                    .WithContentionStats(CaptureLevel.Informational)
                    .WithGcStats(CaptureLevel.Verbose)
                    .WithThreadPoolStats(CaptureLevel.Informational)
                    .WithExceptionStats(CaptureLevel.Errors)
                    .WithJitStats();
            }

            if (_options.UseDebuggingMetrics)
            {
                //_logger.LogInformation("Using debugging metrics.");
                builder.WithDebuggingMetrics(true);
            }

            //_logger.LogInformation("Starting prometheus-net.DotNetRuntime...");

            return builder
                .StartCollecting();
        }


    }
}
