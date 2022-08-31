namespace MauiApp1.Model;

using SQLite;
using System;


public class Activity
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Date { get; set; }
    public string Time { get; set; }
    public ActivityState ActivityState { get; set; }
}
