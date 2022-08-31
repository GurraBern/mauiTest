using Microsoft.Maui.Graphics.Text;
using MauiApp1.Model;
using static System.Net.Mime.MediaTypeNames;

public class ActivityIndicator
{
    private Activity _model;
    private Button _boxIndicator;

    public ActivityIndicator(Activity model)
    {
        this._model = model;
        InitActivityBoxNew();
    }


    //TODO Change to xaml
    private void InitActivityBoxNew()
    {
        this._boxIndicator = new Button();
        SetIndicatorBoxColor(this._model.ActivityState);

        _boxIndicator.FontAttributes = FontAttributes.Bold;
        _boxIndicator.FontSize = 14;
        _boxIndicator.Padding = 0;
        _boxIndicator.WidthRequest = 30;
        _boxIndicator.HeightRequest = 30;
        _boxIndicator.CornerRadius = 5;
        _boxIndicator.TextColor = Color.FromRgb(255,255,255);
        _boxIndicator.Text = SplitToDay(_model.Date);

    }

    private Color SetIndicatorBoxColor(ActivityState state) => state switch
    {
        ActivityState.PRESENT => new Color(116, 255, 112),
        ActivityState.RESTDAY => new Color(255, 203, 76),
        _ => new Color(0, 0, 0, 0.1f)
    };

    public void SetActivityStatus(ActivityState activityState)
    {
        _model.ActivityState = activityState;
        _boxIndicator.BackgroundColor = SetIndicatorBoxColor(activityState);
    }

    public void SetDate(DateTime date)
    {
        _model.Date = date.ToShortDateString();
    }
    private string SplitToDay(string date)
    {
        var splitString = date.Split("-");

        string dayString = splitString[splitString.Count() - 1];

        return dayString;
    }

    public string GetDate()
    {
        return _model.Date;
    }

    public Button GetBoxIndicator()
    {
        return _boxIndicator;
    }
}

