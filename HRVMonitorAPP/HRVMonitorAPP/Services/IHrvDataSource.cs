using HRVMonitorAPP.Models;

namespace HRVMonitorAPP.Services;

/// <summary>
/// Abstraction over a physiological data source.
/// <para>
/// INTEGRATION POINT (device): the app ships with <see cref="SimulatedHrvDataSource"/>. To use a real
/// sensor (e.g. Polar H10 over Bluetooth LE) implement this interface and change the
/// <c>IHrvDataSource</c> registration in <c>MauiProgram.cs</c>. No UI or storage code needs to change.
/// </para>
/// </summary>
public interface IHrvDataSource
{
    /// <summary>True while the source is producing readings.</summary>
    bool IsRunning { get; }

    /// <summary>Raised for every new reading (may be raised from a background thread).</summary>
    event Action<HrvMeasurement>? MeasurementReceived;

    /// <summary>Begins producing readings; no-op if already running.</summary>
    void Start();

    /// <summary>Stops producing readings; no-op if already stopped.</summary>
    void Stop();
}
