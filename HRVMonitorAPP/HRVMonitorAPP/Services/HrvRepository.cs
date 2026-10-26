using HRVMonitorAPP.Models;
using SQLite;

namespace HRVMonitorAPP.Services;

public interface IHrvRepository
{
    Task AddAsync(HrvMeasurement m);
    Task<List<HrvMeasurement>> GetRangeAsync(DateTime from, DateTime to);
    Task<List<HrvMeasurement>> GetLatestAsync(int count);
    Task AddScheduleAsync(MeditationSchedule s);
    Task<List<MeditationSchedule>> GetSchedulesAsync();
    Task UpdateScheduleAsync(MeditationSchedule s);
    Task DeleteScheduleAsync(int id);
}

public sealed class HrvRepository : IHrvRepository
{
    private readonly SQLiteAsyncConnection _db;
    private readonly Task _init;

    public HrvRepository(string dbPath)
    {
        _db = new SQLiteAsyncConnection(dbPath);
        _init = InitAsync();
    }

    private async Task InitAsync()
    {
        await _db.CreateTableAsync<HrvMeasurement>();
        await _db.CreateTableAsync<MeditationSchedule>();
    }

    public async Task AddAsync(HrvMeasurement m) { await _init; await _db.InsertAsync(m); }

    public async Task<List<HrvMeasurement>> GetRangeAsync(DateTime from, DateTime to)
    {
        await _init;
        return await _db.Table<HrvMeasurement>()
            .Where(x => x.Timestamp >= from && x.Timestamp <= to)
            .OrderBy(x => x.Timestamp).ToListAsync();
    }

    public async Task<List<HrvMeasurement>> GetLatestAsync(int count)
    {
        await _init;
        var list = await _db.Table<HrvMeasurement>().OrderByDescending(x => x.Timestamp).Take(count).ToListAsync();
        list.Reverse();
        return list;
    }

    public async Task AddScheduleAsync(MeditationSchedule s) { await _init; await _db.InsertAsync(s); }

    public async Task<List<MeditationSchedule>> GetSchedulesAsync()
    {
        await _init;
        return await _db.Table<MeditationSchedule>().OrderByDescending(x => x.CreatedDate).ToListAsync();
    }

    public async Task UpdateScheduleAsync(MeditationSchedule s) { await _init; await _db.UpdateAsync(s); }

    public async Task DeleteScheduleAsync(int id) { await _init; await _db.DeleteAsync<MeditationSchedule>(id); }
}
