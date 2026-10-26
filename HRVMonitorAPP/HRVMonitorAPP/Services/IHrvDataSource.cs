using HRVMonitorAPP.Models;

namespace HRVMonitorAPP.Services;

/// <summary>
/// Abstraction over a physiological data source. Replace the simulator registration
/// in MauiProgram with a Polar H10 BLE implementation without touching the rest of the app.
/// </summary>
public interface IHrvDataSource
{
    bool IsRunning { get; }
    event Action<HrvMeasurement>? MeasurementReceived;
    void Start();
    void Stop();
}
