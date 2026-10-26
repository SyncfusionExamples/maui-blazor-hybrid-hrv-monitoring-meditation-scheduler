using HRVMonitorAPP.Models;
using SQLite;

namespace HRVMonitorAPP.Services;

/// <summary>
/// Data-access contract for HRV readings and meditation schedules.
/// <para>
/// INTEGRATION POINT (database): the app uses SQLite via <see cref="HrvRepository"/>. To use your own
/// database (SQL Server, PostgreSQL, a REST API, ...) implement this interface and change the
/// <c>IHrvRepository</c> registration in <c>MauiProgram.cs</c>.
/// </para>
/// </summary>
public interface IHrvRepository
{
    /// <summary>Stores a new reading.</summary>
    Task AddAsync(HrvMeasurement m);

    /// <summary>Returns readings with <c>from &lt;= Timestamp &lt;= to</c>, oldest first.</summary>
    Task<List<HrvMeasurement>> GetRangeAsync(DateTime from, DateTime to);

    /// <summary>Returns the most recent <paramref name="count"/> readings, oldest first.</summary>
    Task<List<HrvMeasurement>> GetLatestAsync(int count);

    /// <summary>Stores a new meditation recommendation.</summary>
    Task AddScheduleAsync(MeditationSchedule s);

    /// <summary>Returns all recommendations, newest first.</summary>
    Task<List<MeditationSchedule>> GetSchedulesAsync();

    /// <summary>Updates an existing recommendation.</summary>
    Task UpdateScheduleAsync(MeditationSchedule s);

    /// <summary>Deletes a recommendation by id.</summary>
    Task DeleteScheduleAsync(int id);
}

/// <summary>
/// SQLite implementation of <see cref="IHrvRepository"/> (sqlite-net).
/// <para>
/// INTEGRATION POINT (database): the connection is created from the file path passed in by
/// <c>MauiProgram.cs</c>. Replace this class (or its connection setup in the constructor) with your
/// own database connection to store data elsewhere.
/// </para>
/// </summary>
public sealed class HrvRepository : IHrvRepository
{
    private readonly SQLiteAsyncConnection _db;
    private readonly Task _init;

    /// <summary>Opens (or creates) the SQLite database and its tables.</summary>
    /// <param name="dbPath">Full path of the database file.</param>
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

    /// <inheritdoc />
    public async Task AddAsync(HrvMeasurement m) { await _init; await _db.InsertAsync(m); }

    /// <inheritdoc />
    public async Task<List<HrvMeasurement>> GetRangeAsync(DateTime from, DateTime to)
    {
        await _init;
        return await _db.Table<HrvMeasurement>()
            .Where(x => x.Timestamp >= from && x.Timestamp <= to)
            .OrderBy(x => x.Timestamp).ToListAsync();
    }

    /// <inheritdoc />
    public async Task<List<HrvMeasurement>> GetLatestAsync(int count)
    {
        await _init;
        var list = await _db.Table<HrvMeasurement>().OrderByDescending(x => x.Timestamp).Take(count).ToListAsync();
        list.Reverse();
        return list;
    }

    /// <inheritdoc />
    public async Task AddScheduleAsync(MeditationSchedule s) { await _init; await _db.InsertAsync(s); }

    /// <inheritdoc />
    public async Task<List<MeditationSchedule>> GetSchedulesAsync()
    {
        await _init;
        return await _db.Table<MeditationSchedule>().OrderByDescending(x => x.CreatedDate).ToListAsync();
    }

    /// <inheritdoc />
    public async Task UpdateScheduleAsync(MeditationSchedule s) { await _init; await _db.UpdateAsync(s); }

    /// <inheritdoc />
    public async Task DeleteScheduleAsync(int id) { await _init; await _db.DeleteAsync<MeditationSchedule>(id); }
}
