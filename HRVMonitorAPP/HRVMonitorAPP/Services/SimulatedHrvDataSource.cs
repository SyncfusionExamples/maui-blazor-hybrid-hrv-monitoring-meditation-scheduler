using HRVMonitorAPP.Models;

namespace HRVMonitorAPP.Services;

/// <summary>
/// Generates one synthetic HRV reading per second using a slow sine drift plus noise.
/// <para>
/// INTEGRATION POINT (device): replace this class with a real <see cref="IHrvDataSource"/>
/// (for example a Polar H10 BLE client) by changing the registration in <c>MauiProgram.cs</c>.
/// </para>
/// </summary>
public sealed class SimulatedHrvDataSource : IHrvDataSource
{
    private readonly Random _rng = new();
    private readonly object _gate = new();
    private CancellationTokenSource? _cts;
    private double _phase;

    /// <inheritdoc />
    public bool IsRunning { get; private set; }
    /// <inheritdoc />
    public event Action<HrvMeasurement>? MeasurementReceived;

    /// <inheritdoc />
    public void Start()
    {
        lock (_gate)
        {
            if (IsRunning) return;
            IsRunning = true;
            _cts = new CancellationTokenSource();
            _ = RunAsync(_cts.Token);
        }
    }

    /// <inheritdoc />
    public void Stop()
    {
        lock (_gate)
        {
            if (!IsRunning) return;
            IsRunning = false;
            _cts?.Cancel();
            _cts = null;
        }
    }

    private async Task RunAsync(CancellationToken ct)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(1));
        try
        {
            while (await timer.WaitForNextTickAsync(ct))
                MeasurementReceived?.Invoke(Generate());
        }
        catch (OperationCanceledException) { }
    }

    private HrvMeasurement Generate()
    {
        // Slow sine drift + noise gives believable trends across all ranges (RMSSD ~ 20-70).
        _phase += 0.05;
        var wave = Math.Sin(_phase);
        var rmssd = Math.Clamp(45 + 22 * wave + Noise(4), 10, 90);
        var hr = (int)Math.Clamp(Math.Round(72 - 8 * wave + Noise(2)), 45, 140);
        var sdnn = Math.Clamp(rmssd * 0.85 + Noise(3), 8, 120);
        var pnn50 = Math.Clamp(rmssd * 0.35 + Noise(2), 0, 100);
        var stress = Math.Clamp(100 - rmssd * 1.2 + Noise(3), 0, 100);
        var readiness = Math.Clamp(40 + rmssd * 0.8 + Noise(3), 0, 100);
        return new HrvMeasurement
        {
            Timestamp = DateTime.Now,
            HeartRate = hr,
            RMSSD = Math.Round(rmssd, 1),
            SDNN = Math.Round(sdnn, 1),
            PNN50 = Math.Round(pnn50, 1),
            StressScore = Math.Round(stress, 1),
            ReadinessScore = Math.Round(readiness, 1)
        };
    }

    private double Noise(double amp) => (_rng.NextDouble() * 2 - 1) * amp;
}
