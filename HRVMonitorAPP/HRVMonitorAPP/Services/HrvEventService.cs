using HRVMonitorAPP.Models;

namespace HRVMonitorAPP.Services;

/// <summary>In-process pub/sub that lets UI components react to new readings and stream state changes.</summary>
public interface IHrvEventService
{
    /// <summary>Raised after a reading has been stored.</summary>
    event Action<HrvMeasurement>? MeasurementPublished;

    /// <summary>Raised when streaming starts or stops.</summary>
    event Action? StreamStateChanged;

    /// <summary>Publishes a reading to subscribers.</summary>
    /// <param name="m">The reading.</param>
    void Publish(HrvMeasurement m);

    /// <summary>Notifies subscribers that the streaming state changed.</summary>
    void NotifyStateChanged();
}

/// <summary>Default <see cref="IHrvEventService"/> implementation.</summary>
public sealed class HrvEventService : IHrvEventService
{
    /// <inheritdoc />
    public event Action<HrvMeasurement>? MeasurementPublished;

    /// <inheritdoc />
    public event Action? StreamStateChanged;

    /// <inheritdoc />
    public void Publish(HrvMeasurement m) => MeasurementPublished?.Invoke(m);
    /// <inheritdoc />
    public void NotifyStateChanged() => StreamStateChanged?.Invoke();
}
