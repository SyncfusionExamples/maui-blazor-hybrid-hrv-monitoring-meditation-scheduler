using HRVMonitorAPP.Models;

namespace HRVMonitorAPP.Services;

/// <summary>Pipes data source -> SQLite -> event service.</summary>
public sealed class HrvStreamingService
{
    private readonly IHrvDataSource _source;
    private readonly IHrvRepository _repo;
    private readonly IHrvEventService _events;

    public HrvStreamingService(IHrvDataSource source, IHrvRepository repo, IHrvEventService events)
    {
        _source = source;
        _repo = repo;
        _events = events;
        _source.MeasurementReceived += OnMeasurement;
    }

    public bool IsStreaming => _source.IsRunning;

    public void Start() { _source.Start(); _events.NotifyStateChanged(); }
    public void Stop() { _source.Stop(); _events.NotifyStateChanged(); }

    private async void OnMeasurement(HrvMeasurement m)
    {
        try
        {
            await _repo.AddAsync(m);
            _events.Publish(m);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to persist measurement: {ex}");
        }
    }
}
