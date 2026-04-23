using PassportMD.Helpers;
using PassportMD.Models;

namespace PassportMD.Pages;

public partial class CreateProfilePage : ContentPage
{
    public CreateProfilePage()
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

        string name = NameEntry.Text?.Trim();
        if (string.IsNullOrWhiteSpace(name))
        {
            App.Audio?.PlayWarning();
            await DisplayAlert("Invalid", "Name cannot be empty.", "OK");
            return;
        }

        if (!int.TryParse(AgeEntry.Text, out int age) || age <= 0)
        {
            App.Audio?.PlayWarning();
            await DisplayAlert("Invalid", "Please enter a valid age.", "OK");
            return;
        }

        if (!double.TryParse(HeightEntry.Text, out double height) || height <= 0)
        {
            App.Audio?.PlayWarning();
            await DisplayAlert("Invalid", "Please enter a valid height in inches.", "OK");
            return;
        }

        if (!double.TryParse(WeightEntry.Text, out double weight) || weight <= 0)
        {
            App.Audio?.PlayWarning();
            await DisplayAlert("Invalid", "Please enter a valid weight in pounds.", "OK");
            return;
        }

        var profile = new PersonProfile(name, age, height, weight);
        MainPage.Passport = new HealthPassport(profile);

        App.Audio?.PlaySuccess();
        await DisplayAlert("Success", $"Profile created for {name}!", "OK");
        await Shell.Current.GoToAsync("//MainPage");
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        App.Audio?.PlayTap();
        await Anim.BouncePress((VisualElement)sender);
        await Shell.Current.GoToAsync("//MainPage");
    }
}
