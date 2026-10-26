using HRVMonitorAPP.Models;

namespace HRVMonitorAPP.Services;

public interface IHrvEventService
{
    event Action<HrvMeasurement>? MeasurementPublished;
    event Action? StreamStateChanged;
    void Publish(HrvMeasurement m);
    void NotifyStateChanged();
}

public sealed class HrvEventService : IHrvEventService
{
    public event Action<HrvMeasurement>? MeasurementPublished;
    public event Action? StreamStateChanged;
    public void Publish(HrvMeasurement m) => MeasurementPublished?.Invoke(m);
    public void NotifyStateChanged() => StreamStateChanged?.Invoke();
}
