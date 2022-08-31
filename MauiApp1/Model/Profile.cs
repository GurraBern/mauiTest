namespace MauiApp1.Model;

using System;
using SQLite;

public class Profile
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string LastDate { get; set; }
    public string LastTime { get; set; }
    public int StreakDays { get; set; }
    public ActivityState ActivityState { get; set; }
}
