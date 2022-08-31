using SQLite;
using MauiApp1.Model;


namespace MauiApp1.Services;

public class DateIndicatorService
{
    static SQLiteAsyncConnection db;

    static async Task Init()
    {

        if (db != null)
        {
            return;
        }

        var databasePath = Path.Combine(FileSystem.AppDataDirectory, "ActivityDates.db");
        db = new SQLiteAsyncConnection(databasePath);
        await db.CreateTableAsync<Activity>();

        var today = DateTime.Today;
        var endOfMonth = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
        var dates = await GetActivityBetween(today.ToShortDateString(), endOfMonth.ToShortDateString());

        if(dates.Count() <= 0)
        {
            await AddDatesToMonth(DateTime.Today);
        }
    }

    public static async Task AddDate(DateTime date, ActivityState activityState)
    {
        await Init();

        var dateString = date.ToShortDateString();
        var activityIndicator = new Activity()
        {
            Date = dateString,
            ActivityState = activityState
        };

        var id = await db.InsertAsync(activityIndicator);
    }

    public static async Task AddDatesToMonth(DateTime date)
    {
        await Init();

        int daysCount = DateTime.DaysInMonth(date.Year, date.Month);
        List<Activity> activityDates = new List<Activity>();

        for (int i = 1; i < daysCount + 1; i++)
        {
            var incDay = new DateTime(date.Year, date.Month, i);
            var activityIndicator = new Activity()
            {
                Date = incDay.ToShortDateString(),
                Time = incDay.ToShortTimeString(),
                ActivityState = ActivityState.ABSENT
            };           

            activityDates.Add(activityIndicator);
        }

        var id = await db.InsertAllAsync(activityDates);
    }

    public static async Task UpdateDate(DateTime date, ActivityState activityState)
    {
        var dateShort = date.ToShortDateString();
        var activityIndicatorObj = await db.Table<Activity>().Where(v => v.Date.Equals(dateShort)).FirstOrDefaultAsync();

        if(activityIndicatorObj != null)
        {
            activityIndicatorObj.ActivityState = activityState;
            activityIndicatorObj.Time = date.ToShortTimeString();
            await db.UpdateAsync(activityIndicatorObj);
        } else
        {
            //TODO throw exeption, cant update unless date exist in db
        }
    }

    public static async Task RemoveDate(int id)
    {
        await Init();
        await db.DeleteAsync<Activity>(id);
    }

    public static async Task<IEnumerable<Activity>> GetActivityDates()
    {
        await Init();
        var dates = await db.Table<Activity>().ToListAsync();

        return dates;
    }

    public static async Task<IEnumerable<Activity>> GetActivityBetween(string startDate, string endDate)
    {
        await Init();
        var dbConnection = db.GetConnection();

        return dbConnection.Query<Activity>($"SELECT * FROM Activity WHERE date BETWEEN '{startDate}' AND '{endDate}'", startDate, endDate);
    }

    public enum DaysOfWeek
    {
        Mon = 0,
        Tue = 1,
        Wed = 2,
        Thu = 3,
        Fri = 4,
        Sat = 5,
        Sun = 6
    };
}