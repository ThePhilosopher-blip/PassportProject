using PassportMD.Helpers;

namespace PassportMD.Pages;

public partial class VitalsPage : ContentPage
{
    public VitalsPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (Content is ScrollView sv && sv.Content is VisualElement layout)
            await Anim.PageContentIn(layout);
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        App.Audio?.PlayTap();
        await Anim.BouncePress((VisualElement)sender);

        if (!int.TryParse(SystolicEntry.Text, out int systolic) || systolic <= 0)
        {
            App.Audio?.PlayWarning();
            await DisplayAlert("Invalid", "Please enter a valid systolic reading.", "OK");
            return;
        }

        if (!int.TryParse(DiastolicEntry.Text, out int diastolic) || diastolic <= 0)
        {
            App.Audio?.PlayWarning();
            await DisplayAlert("Invalid", "Please enter a valid diastolic reading.", "OK");
            return;
        }

        if (!int.TryParse(HeartRateEntry.Text, out int heartRate) || heartRate <= 0)
        {
            App.Audio?.PlayWarning();
            await DisplayAlert("Invalid", "Please enter a valid heart rate.", "OK");
            return;
        }

        MainPage.Passport.RecordVitals(systolic, diastolic, heartRate);

        App.Audio?.PlaySuccess();
        await DisplayAlert("Success",
            $"Vitals recorded: BP {MainPage.Passport.Vitals.GetReading()}, HR {heartRate} bpm",
            "OK");
        await Shell.Current.GoToAsync("//MainPage");
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        App.Audio?.PlayTap();
        await Anim.BouncePress((VisualElement)sender);
        await Shell.Current.GoToAsync("//MainPage");
    }
}
