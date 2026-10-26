using HRVMonitorAPP.Services;
using Microsoft.Extensions.Logging;
using Syncfusion.Blazor;

namespace HRVMonitorAPP
{
    /// <summary>Application bootstrapper and dependency-injection composition root.</summary>
    public static class MauiProgram
    {
        /// <summary>Builds the MAUI app and registers all services.</summary>
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddSyncfusionBlazor();

            // INTEGRATION POINT (database): register your own IHrvRepository implementation here
            // (SQL Server, PostgreSQL, REST API, ...) instead of the bundled SQLite repository.
            builder.Services.AddSingleton<IHrvRepository>(_ =>
                new HrvRepository(Path.Combine(FileSystem.AppDataDirectory, "hrv.db3")));
            builder.Services.AddSingleton<IHrvEventService, HrvEventService>();
            // INTEGRATION POINT (device): replace SimulatedHrvDataSource with your real IHrvDataSource
            // (e.g. a Polar H10 Bluetooth LE client) to stream actual data.
            builder.Services.AddSingleton<IHrvDataSource, SimulatedHrvDataSource>();
            builder.Services.AddSingleton<HrvStreamingService>();
            builder.Services.AddSingleton<IMeditationService, MeditationService>();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            var app = builder.Build();
            app.Services.GetRequiredService<HrvStreamingService>().Start();
            return app;
        }
    }
}
