# MAUI Blazor Hybrid HRV Monitoring & Meditation Scheduler

## Description and Purpose

A cross-platform wellness application built with **.NET MAUI Blazor Hybrid** that is designed to capture physiological data from Bluetooth LE chest straps (such as the Polar H10), analyze heart rate variability (HRV), visualize HRV over time, and schedule meditation experiments based on the measured results.

The proof of concept targets **Windows and macOS (Mac Catalyst)** desktops, and because the UI is written in Blazor (Razor components hosted in a `BlazorWebView`) with a single MAUI project, it can be ported to **Android and iOS** with no UI rewrite.

> **Project status:** The repository currently contains the MAUI Blazor Hybrid app scaffold (shell, navigation, platform heads for Windows, macOS, Android and iOS). The HRV, charting and scheduling features below describe the intended design and show how each piece plugs into this structure. Syncfusion packages are not yet referenced in the `.csproj`.

## Features

### 1. Cross-platform MAUI Blazor Hybrid host

The MAUI app hosts the Blazor UI in a `BlazorWebView` (`MainPage.xaml`), and services are registered in `MauiProgram.cs`:

```csharp
builder.Services.AddMauiBlazorWebView();

#if DEBUG
builder.Services.AddBlazorWebViewDeveloperTools();
builder.Logging.AddDebug();
#endif
```

The project multi-targets `net10.0-android`, `net10.0-ios`, `net10.0-maccatalyst` and `net10.0-windows10.0.19041.0`.

### 2. Chest strap data acquisition (planned)

Read heart rate and R-R intervals from a Polar H10 over the standard BLE Heart Rate Service (`0x180D`, measurement characteristic `0x2A37`). Hide the BLE library behind an interface so each platform supplies its own implementation:

```csharp
public interface IHeartRateSensor
{
    event Action<HeartRateSample> SampleReceived;   // BPM + R-R intervals (ms)
    Task ConnectAsync(string deviceId);
    Task DisconnectAsync();
}

builder.Services.AddSingleton<IHeartRateSensor, PolarH10Sensor>();
```

### 3. HRV analysis (planned)

Compute standard time-domain HRV metrics from the R-R intervals:

```csharp
public static double Rmssd(IReadOnlyList<double> rr) =>
    Math.Sqrt(rr.Zip(rr.Skip(1), (a, b) => Math.Pow(b - a, 2)).Average());
```

### 4. HRV charts over time (planned)

Razor pages bind the computed metrics to a Syncfusion chart:

```razor
<SfChart>
    <ChartSeriesCollection>
        <ChartSeries DataSource="@hrvPoints" XName="Time" YName="Rmssd" Type="ChartSeriesType.Line" />
    </ChartSeriesCollection>
</SfChart>
```

### 5. Meditation scheduler (planned)

Plan meditation sessions/experiments based on HRV results (for example, recommending a session when RMSSD drops below the user's baseline) and display them in a calendar/scheduler view.

```razor
<SfSchedule TValue="MeditationSession" @bind-SelectedDate="@today">
    <ScheduleEvents TValue="MeditationSession" />
    <ScheduleEventSettings DataSource="@sessions" />
</SfSchedule>
```

### 6. Navigation and layout

`Components/Layout/NavMenu.razor` provides the side navigation and `MainLayout.razor` the page shell. Add pages under `Components/Pages` with `@page` routes and link them from the menu:

```razor
<NavLink class="nav-link" href="hrv">HRV Dashboard</NavLink>
```

## Folder Structure

```
maui-blazor-hybrid-hrv-monitoring-meditation-scheduler/
├── README.md
└── HRVMonitorAPP/
    ├── HRVMonitorAPP.slnx                  # Solution file
    └── HRVMonitorAPP/                      # MAUI Blazor Hybrid project
        ├── HRVMonitorAPP.csproj            # Multi-target project (Android, iOS, MacCatalyst, Windows)
        ├── App.xaml / App.xaml.cs          # Application entry and window creation
        ├── MainPage.xaml / .cs             # Hosts the BlazorWebView
        ├── MauiProgram.cs                  # App builder and service registration
        ├── Components/
        │   ├── _Imports.razor              # Global Razor usings
        │   ├── Routes.razor                # Blazor router
        │   ├── Layout/                     # MainLayout, NavMenu (+ scoped CSS)
        │   └── Pages/                      # Home, Counter, Weather, NotFound
        ├── Platforms/
        │   ├── Android/                    # MainActivity, MainApplication, AndroidManifest.xml
        │   ├── iOS/                        # AppDelegate, Info.plist, PrivacyInfo.xcprivacy
        │   ├── MacCatalyst/                # AppDelegate, Info.plist, Entitlements.plist
        │   └── Windows/                    # App.xaml, Package.appxmanifest, app.manifest
        ├── Properties/launchSettings.json
        ├── Resources/                      # AppIcon, Fonts, Images, Raw, Splash
        └── wwwroot/                        # index.html, app.css, Bootstrap
```

## How to Run the Sample

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download) with the MAUI workload: `dotnet workload install maui`
- Visual Studio 2022/2026 with the **.NET Multi-platform App UI development** workload (Windows), or Visual Studio Code with the .NET MAUI extension
- For macOS (Mac Catalyst) and iOS builds: a Mac with Xcode
- A Bluetooth LE chest strap such as the Polar H10 (needed once the sensor integration is added)
- A Syncfusion license key (Community License or trial) once Syncfusion components are added

### Steps

1. Clone the repository:
   ```bash
   git clone https://github.com/SyncfusionExamples/maui-blazor-hybrid-hrv-monitoring-meditation-scheduler.git
   cd maui-blazor-hybrid-hrv-monitoring-meditation-scheduler/HRVMonitorAPP
   ```
2. Open `HRVMonitorAPP.slnx` in Visual Studio, or restore from the command line:
   ```bash
   dotnet restore
   ```
3. Run the app:
   - **Windows:** `dotnet build -t:Run -f net10.0-windows10.0.19041.0 HRVMonitorAPP/HRVMonitorAPP.csproj`
   - **Mac Catalyst:** `dotnet build -t:Run -f net10.0-maccatalyst HRVMonitorAPP/HRVMonitorAPP.csproj`
   - **Android:** `dotnet build -t:Run -f net10.0-android HRVMonitorAPP/HRVMonitorAPP.csproj`
   - **iOS:** `dotnet build -t:Run -f net10.0-ios HRVMonitorAPP/HRVMonitorAPP.csproj`
4. Register the Syncfusion license (when using Syncfusion components) in `MauiProgram.cs`:
   ```csharp
   Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("YOUR LICENSE KEY");
   ```

## Syncfusion Components Used

| Component | Used for | Documentation / demo |
| --- | --- | --- |
| `Grid` | Live and recorded HRV data grid | [Docs](https://blazor.syncfusion.com/documentation/datagrid/getting-started) · [Demo](https://blazor.syncfusion.com/demos/datagrid/overview) |
| `Chart` | Multi-line HRV chart | [Docs](https://blazor.syncfusion.com/documentation/chart/getting-started) · [Demo](https://blazor.syncfusion.com/demos/chart/line) |
| `Schedule` | Meditation Scheduler (Day/Week/Month/Agenda) | [Docs](https://blazor.syncfusion.com/documentation/scheduler/getting-started) · [Demo](https://blazor.syncfusion.com/demos/scheduler/default-functionalities) |
| `DateTimePicker` | From/To filter for recorded data | [Docs](https://blazor.syncfusion.com/documentation/datetime-picker/getting-started) · [Demo](https://blazor.syncfusion.com/demos/datetimepicker/default-functionalities) |
| `Button` | Actions and mode toggles | [Docs](https://blazor.syncfusion.com/documentation/button/getting-started) · [Demo](https://blazor.syncfusion.com/demos/button/default-functionalities) |
| Badge (`e-badge` CSS class) | LIVE / STOPPED indicator | [Docs](https://blazor.syncfusion.com/documentation/badge/getting-started) · [Demo](https://blazor.syncfusion.com/demos/badge/default-functionalities) |
| Bootstrap 5 theme (`Syncfusion.Blazor.Themes`) | Consistent styling | [Docs](https://blazor.syncfusion.com/documentation/appearance/themes) |
