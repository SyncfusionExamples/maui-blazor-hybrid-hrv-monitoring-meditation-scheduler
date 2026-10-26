# HRV Monitoring & Meditation Scheduler (.NET MAUI Blazor Hybrid)

## Overview

A cross-platform wellness proof of concept built with .NET MAUI Blazor Hybrid and Syncfusion Blazor components. It streams simulated heart rate variability (HRV) data from a wearable chest strap, stores it in SQLite, visualizes it in real time, and recommends meditation sessions based on HRV metrics.

**Key features**

- **Simulated HRV streaming:** one reading per second (Heart Rate, RMSSD, SDNN, PNN50, Stress Score, Readiness Score), stored in SQLite. The data source sits behind `IHrvDataSource`, so a real device such as the Polar H10 can replace it.
- **Controller Dashboard:**
  - Start/Stop streaming and a Live Data / Recorded Data toggle.
  - Live KPI cards and a data grid with paging, sorting, filtering and Excel/CSV/PDF export.
  - An interactive multi-line chart (Heart Rate, RMSSD, Stress Score) sharing one Timestamp axis.
- **Recorded Data:** pick a From/To date-time, load matching records from SQLite, and see the grid, chart and historical KPI cards (average/max heart rate, average/max RMSSD, average stress, average readiness) update for that range.
- **Meditation Scheduler:** recommendations from average RMSSD (below 30: 20-minute stress recovery; 30-50: 10-minute mindfulness; above 50: maintenance session), saved to SQLite and shown in a scheduler with Day, Week, Month and Agenda views. Sessions can be dragged, resized, edited or deleted.
- **Database:** `HRVMeasurements` and `MeditationSchedules` tables.

## Syncfusion Components Used

| Component | Used for | Documentation / demo |
| --- | --- | --- |
| `SfGrid` | Live and recorded HRV data grid | [Docs](https://blazor.syncfusion.com/documentation/datagrid/getting-started) · [Demo](https://blazor.syncfusion.com/demos/datagrid/overview) |
| `SfChart` | Multi-line HRV chart | [Docs](https://blazor.syncfusion.com/documentation/chart/getting-started) · [Demo](https://blazor.syncfusion.com/demos/chart/line) |
| `SfSchedule` | Meditation Scheduler (Day/Week/Month/Agenda) | [Docs](https://blazor.syncfusion.com/documentation/scheduler/getting-started) · [Demo](https://blazor.syncfusion.com/demos/scheduler/default-functionalities) |
| `SfDateTimePicker` | From/To filter for recorded data | [Docs](https://blazor.syncfusion.com/documentation/datetime-picker/getting-started) · [Demo](https://blazor.syncfusion.com/demos/datetimepicker/default-functionalities) |
| `SfButton` | Actions and mode toggles | [Docs](https://blazor.syncfusion.com/documentation/button/getting-started) · [Demo](https://blazor.syncfusion.com/demos/button/default-functionalities) |
| `SfCard` | KPI and suggestion cards | [Docs](https://blazor.syncfusion.com/documentation/card/getting-started) · [Demo](https://blazor.syncfusion.com/demos/card/default-functionalities) |
| `SfToast` | Notifications | [Docs](https://blazor.syncfusion.com/documentation/toast/getting-started) · [Demo](https://blazor.syncfusion.com/demos/toast/default-functionalities) |
| `SfSkeleton` | Loading placeholders | [Docs](https://blazor.syncfusion.com/documentation/skeleton/getting-started) · [Demo](https://blazor.syncfusion.com/demos/skeleton/default-functionalities) |
| Badge (`e-badge` CSS class) | LIVE / STOPPED indicator | [Docs](https://blazor.syncfusion.com/documentation/badge/getting-started) · [Demo](https://blazor.syncfusion.com/demos/badge/default-functionalities) |
| Bootstrap 5 theme (`Syncfusion.Blazor.Themes`) | Consistent styling | [Docs](https://blazor.syncfusion.com/documentation/appearance/themes) |

## Prerequisites

- Visual Studio 2022 (with the **.NET Multi-platform App UI development** workload) or Visual Studio Code (with the .NET MAUI extension)
- .NET SDK 10.0 or later (the project targets `net10.0`)
- Syncfusion Blazor license key, required to remove the trial banner. Get one from the [Syncfusion license page](https://help.syncfusion.com/common/essential-studio/licensing/overview) and register it with `SyncfusionLicenseProvider.RegisterLicense("YOUR_KEY")` in `MauiProgram.cs`.

## How to Run the Project

### Visual Studio

1. Clone or download this repository.
2. Open `HRVMonitorAPP/HRVMonitorAPP.slnx` (the solution file).
3. Restore all NuGet packages.
4. Ensure the `HRVMonitorAPP` project is selected as the startup project.
5. Build the solution.
6. Run the application using **Ctrl + F5** (Start Without Debugging).
