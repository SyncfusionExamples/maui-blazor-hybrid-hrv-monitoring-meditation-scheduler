using HRVMonitorAPP.Models;

namespace HRVMonitorAPP.Services;

/// <summary>Pipes <see cref="IHrvDataSource"/> readings into <see cref="IHrvRepository"/> and then publishes them to the UI.</summary>
public sealed class HrvStreamingService
{
    private readonly IHrvDataSource _source;
    private readonly IHrvRepository _repo;
    private readonly IHrvEventService _events;

    /// <summary>Creates the service and subscribes to the data source.</summary>
    /// <param name="source">Device or simulator producing readings.</param>
    /// <param name="repo">Persistence layer.</param>
    /// <param name="events">UI notification hub.</param>
    public HrvStreamingService(IHrvDataSource source, IHrvRepository repo, IHrvEventService events)
    {
        _source = source;
        _repo = repo;
        _events = events;
        _source.MeasurementReceived += OnMeasurement;
    }

    /// <summary>True while the underlying data source is running.</summary>
    public bool IsStreaming => _source.IsRunning;

    /// <summary>Starts the data source and notifies the UI.</summary>
    public void Start() { _source.Start(); _events.NotifyStateChanged(); }
    /// <summary>Stops the data source and notifies the UI.</summary>
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
