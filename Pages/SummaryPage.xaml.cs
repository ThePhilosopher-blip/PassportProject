using PassportMD.Helpers;

namespace PassportMD.Pages;

public partial class SummaryPage : ContentPage
{
    public SummaryPage()
    {
        InitializeComponent();
        LoadSummary();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await Anim.FadeSlideIn(PageTitle, 400);

        await Anim.StaggerIn(new VisualElement[]
        {
            ProfileFrame, BmiFrame, BpFrame, HrFrame, FlagsFrame, BackBtn
        }, 400, 100);

        var passport = MainPage.Passport;
        if (passport != null && passport.GetHealthFlags().Count > 0)
        {
            App.Audio?.PlayWarning();
            await Anim.AttentionPulse(FlagsFrame);
        }
    }

    private void LoadSummary()
    {
        var passport = MainPage.Passport;
        if (passport == null) return;

        var profile = passport.Profile;

        // Profile
        NameLabel.Text = $"Name:   {profile.Name}";
        AgeLabel.Text = $"Age:    {profile.Age}";
        HeightLabel.Text = $"Height: {profile.Height} inches";
        WeightLabel.Text = $"Weight: {profile.Weight} lbs";

        // BMI
        double bmi = profile.CalculateBmi();
        string bmiCategory = profile.GetBmiCategory();
        BmiLabel.Text = $"BMI: {bmi:F1}";
        BmiCategoryLabel.Text = $"Category: {bmiCategory}";
        BmiCategoryLabel.TextColor = GetHealthColor(bmiCategory);
        BmiTipLabel.Text = profile.GetBmiTip();
        BmiFrame.BackgroundColor = GetHealthTint(bmiCategory);

        // Blood Pressure & Heart Rate
        if (passport.HasVitals())
        {
            string bpCategory = passport.Vitals.GetBpCategory();
            BpLabel.Text = $"Reading: {passport.Vitals.GetReading()}";
            BpCategoryLabel.Text = $"Category: {bpCategory}";
            BpCategoryLabel.TextColor = GetHealthColor(bpCategory);
            BpTipLabel.Text = passport.Vitals.GetBpTip();
            BpFrame.BackgroundColor = GetHealthTint(bpCategory);

            string hrCategory = passport.Vitals.GetHeartRateCategory();
            HrLabel.Text = $"Rate: {passport.Vitals.HeartRate} bpm";
            HrCategoryLabel.Text = $"Category: {hrCategory}";
            HrCategoryLabel.TextColor = GetHealthColor(hrCategory);
            HrTipLabel.Text = passport.Vitals.GetHeartRateTip();
            HrFrame.BackgroundColor = GetHealthTint(hrCategory);
        }
        else
        {
            BpLabel.Text = "Not yet recorded";
            BpCategoryLabel.IsVisible = false;
            BpTipLabel.IsVisible = false;

            HrLabel.Text = "Not yet recorded";
            HrCategoryLabel.IsVisible = false;
            HrTipLabel.IsVisible = false;
        }

        // Health Flags
        var flags = passport.GetHealthFlags();
        if (flags.Count > 0)
        {
            FlagsTitle.Text = "Health Flags";
            FlagsTitle.TextColor = Color.FromArgb("#e94560");
            FlagsLabel.Text = string.Join("\n", flags.Select(f => $"* {f}"));
            FlagsFrame.BackgroundColor = Color.FromArgb("#fdf0f2");
        }
        else
        {
            FlagsTitle.Text = "No Health Flags";
            FlagsTitle.TextColor = Color.FromArgb("#2e8b57");
            FlagsLabel.Text = "All readings are in healthy ranges!";
            FlagsFrame.BackgroundColor = Color.FromArgb("#f0f7f4");
        }
    }

    private static Color GetHealthColor(string category)
    {
        if (category == "Healthy" || category == "Normal")
            return Color.FromArgb("#2e8b57");

        if (category == "Obese" || category.Contains("Stage 2") || category.Contains("Tachycardia"))
            return Color.FromArgb("#e94560");

        return Color.FromArgb("#e8a317");
    }

    private static Color GetHealthTint(string category)
    {
        if (category == "Healthy" || category == "Normal")
            return Color.FromArgb("#f0f7f4");

        if (category == "Obese" || category.Contains("Stage 2") || category.Contains("Tachycardia"))
            return Color.FromArgb("#fdf0f2");

        return Color.FromArgb("#fdf6e8");
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        App.Audio?.PlayTap();
        await Anim.BouncePress((VisualElement)sender);
        await Shell.Current.GoToAsync("//MainPage");
    }
}
