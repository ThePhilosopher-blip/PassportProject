using PassportMD.Helpers;

namespace PassportMD.Pages;

public partial class UpdateWeightPage : ContentPage
{
    public UpdateWeightPage()
    {
        InitializeComponent();
        CurrentWeightLabel.Text = $"Current weight: {MainPage.Passport.Profile.Weight} lbs";
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (Content is ScrollView sv && sv.Content is VisualElement layout)
            await Anim.PageContentIn(layout);
    }

    private async void OnUpdateClicked(object sender, EventArgs e)
    {
        App.Audio?.PlayTap();
        await Anim.BouncePress((VisualElement)sender);

        if (!double.TryParse(WeightEntry.Text, out double weight) || weight <= 0)
        {
            App.Audio?.PlayWarning();
            await DisplayAlert("Invalid", "Please enter a valid weight in pounds.", "OK");
            return;
        }

        MainPage.Passport.Profile.Weight = weight;
        App.Audio?.PlaySuccess();
        await DisplayAlert("Success", $"Weight updated to {weight} lbs!", "OK");
        await Shell.Current.GoToAsync("//MainPage");
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        App.Audio?.PlayTap();
        await Anim.BouncePress((VisualElement)sender);
        await Shell.Current.GoToAsync("//MainPage");
    }
}
