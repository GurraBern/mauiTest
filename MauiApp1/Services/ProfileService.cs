using SQLite;
using MauiApp1.Model;

namespace MauiApp1.Services;

public class ProfileService
{

    static SQLiteAsyncConnection db;

    static async Task Init()
    {
        if (db != null)
        {
            return;
        }

        var databasePath = Path.Combine(FileSystem.AppDataDirectory, "Profile.db");
        db = new SQLiteAsyncConnection(databasePath);
        await db.CreateTableAsync<Profile>();

        var profile = await db.Table<Profile>().FirstOrDefaultAsync();
        if (profile == null)
        {
            Profile newProfile = new Profile
            {
                Name = "Gustav",
                LastName = "Berndtzen",
                LastDate = DateTime.MinValue.ToShortDateString(),
                LastTime = DateTime.MinValue.ToShortTimeString(),
                ActivityState = ActivityState.ABSENT,
                StreakDays = 0,
            };
            await db.InsertAsync(newProfile);
        }
    }

    public static async Task UpdateLatestActivity(DateTime date, ActivityState activityState)
{
        await Init();                                                                                                     
        var profile = await db.Table<Profile>().FirstOrDefaultAsync();

        if(activityState == ActivityState.PRESENT || activityState == ActivityState.RESTDAY)
        {
            //TODO remove later
            profile.StreakDays++;

            //TODO Uncomment
            //if (!profile.LastDate.Equals(DateTime.Now.ToShortDateString()))
            //{
            //    profile.StreakDays++;
            //}
        }
        else
        {
            profile.StreakDays = 0;
        }

        profile.ActivityState = activityState;
        profile.LastDate = date.ToShortDateString();
        profile.LastTime = date.ToShortTimeString();
        await db.UpdateAsync(profile);
    }

    public static async Task<Profile> GetProfile()
    {
        await Init();

        return db.Table<Profile>().FirstAsync().Result;
    }

    public static async Task<int> GetCurrentActivityStreakAsync()
    {
        await Init();

        return db.Table<Profile>().FirstAsync().Result.StreakDays;
    }
}