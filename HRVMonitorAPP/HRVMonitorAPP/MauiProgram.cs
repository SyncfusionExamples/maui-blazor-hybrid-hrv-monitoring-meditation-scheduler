using HRVMonitorAPP.Services;
using Microsoft.Extensions.Logging;
using Syncfusion.Blazor;

namespace HRVMonitorAPP
{
    public static class MauiProgram
    {
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

            builder.Services.AddSingleton<IHrvRepository>(_ =>
                new HrvRepository(Path.Combine(FileSystem.AppDataDirectory, "hrv.db3")));
            builder.Services.AddSingleton<IHrvEventService, HrvEventService>();
            // Swap for a Polar H10 BLE implementation of IHrvDataSource later.
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
