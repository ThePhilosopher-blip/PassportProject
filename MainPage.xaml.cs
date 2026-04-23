using PassportMD.Helpers;
using PassportMD.Services;

namespace PassportMD;

public partial class MainPage : ContentPage
{
    public static Models.HealthPassport? Passport { get; set; }

    private readonly CsvRepository _repo = new CsvRepository();
    private bool _hasAnimated;

    public MainPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        UpdateStatus();

        if (!_hasAnimated)
        {
            _hasAnimated = true;
            App.Audio?.StartMusic();

            await Anim.FadeSlideIn(TitleLabel, 500);
            await Anim.FadeSlideIn(SubtitleLabel, 350, 50);
            await Anim.FadeSlideIn(StatusCard, 400, 50);

            await Anim.StaggerIn(new VisualElement[]
            {
                CreateProfileBtn, EnterVitalsBtn, ViewSummaryBtn, UpdateWeightBtn,
                SaveBtn, LoadBtn, ClearBtn
            }, 300, 60);
        }
        else
        {
            Anim.EnsureVisible(TitleLabel, SubtitleLabel, StatusCard,
                CreateProfileBtn, EnterVitalsBtn, ViewSummaryBtn, UpdateWeightBtn,
                SaveBtn, LoadBtn, ClearBtn);
        }
    }

    private void UpdateStatus()
    {
        if (Passport != null)
        {
            StatusTitle.Text = Passport.Profile.Name;
            var bmi = Passport.Profile.CalculateBmi();
            var info = $"BMI: {bmi:F1} ({Passport.Profile.GetBmiCategory()})";
            if (Passport.HasVitals())
                info += $"\nBP: {Passport.Vitals.GetReading()}  |  HR: {Passport.Vitals.HeartRate} bpm";
            StatusDetail.Text = info;
            StatusCard.BackgroundColor = Color.FromArgb("#f0f7f4");
        }
        else
        {
            StatusTitle.Text = "Welcome";
            StatusDetail.Text = "Create a profile to get started.";
            StatusCard.BackgroundColor = Color.FromArgb("#f5f5f5");
        }
    }

    private async void OnCreateProfileClicked(object sender, EventArgs e)
    {
        App.Audio?.PlayTap();
        await Anim.BouncePress((VisualElement)sender);
        await Shell.Current.GoToAsync("CreateProfile");
    }

    private async void OnEnterVitalsClicked(object sender, EventArgs e)
    {
        if (Passport == null)
        {
            App.Audio?.PlayWarning();
            await DisplayAlert("No Profile", "Please create a profile first!", "OK");
            return;
        }
        App.Audio?.PlayTap();
        await Anim.BouncePress((VisualElement)sender);
        await Shell.Current.GoToAsync("Vitals");
    }

    private async void OnViewSummaryClicked(object sender, EventArgs e)
    {
        if (Passport == null)
        {
            App.Audio?.PlayWarning();
            await DisplayAlert("No Profile", "Please create a profile first!", "OK");
            return;
        }
        App.Audio?.PlayTap();
        await Anim.BouncePress((VisualElement)sender);
        await Shell.Current.GoToAsync("Summary");
    }

    private async void OnUpdateWeightClicked(object sender, EventArgs e)
    {
        if (Passport == null)
        {
            App.Audio?.PlayWarning();
            await DisplayAlert("No Profile", "Please create a profile first!", "OK");
            return;
        }
        App.Audio?.PlayTap();
        await Anim.BouncePress((VisualElement)sender);
        await Shell.Current.GoToAsync("UpdateWeight");
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (Passport == null)
        {
            App.Audio?.PlayWarning();
            await DisplayAlert("No Profile", "Please create a profile first!", "OK");
            return;
        }

        App.Audio?.PlayTap();
        await Anim.BouncePress((VisualElement)sender);
        _repo.Save(Passport);
        App.Audio?.PlaySave();
        await Anim.SuccessPulse(StatusCard);
        await DisplayAlert("Saved",
            $"Passport for {Passport.Profile.Name} saved to:\n{_repo.FolderPath}",
            "OK");
    }

    private async void OnLoadClicked(object sender, EventArgs e)
    {
        App.Audio?.PlayTap();
        await Anim.BouncePress((VisualElement)sender);

        string[] files = _repo.GetSavedFiles();

        if (files.Length == 0)
        {
            App.Audio?.PlayWarning();
            await DisplayAlert("No Saved Passports", "No passport files found.", "OK");
            return;
        }

        string[] displayNames = new string[files.Length];
        for (int i = 0; i < files.Length; i++)
            displayNames[i] = Path.GetFileNameWithoutExtension(files[i]);

        string chosen = await DisplayActionSheet("Load Passport", "Cancel", null, displayNames);

        if (string.IsNullOrEmpty(chosen) || chosen == "Cancel")
            return;

        for (int i = 0; i < displayNames.Length; i++)
        {
            if (displayNames[i] == chosen)
            {
                var loaded = _repo.Load(files[i]);
                if (loaded != null)
                {
                    Passport = loaded;
                    UpdateStatus();
                    App.Audio?.PlaySuccess();
                    await Anim.CelebrationBounce(StatusCard);
                    await DisplayAlert("Loaded", $"Passport for {Passport.Profile.Name} loaded!", "OK");
                }
                else
                {
                    App.Audio?.PlayWarning();
                    await DisplayAlert("Error", "Could not read that passport file.", "OK");
                }
                return;
            }
        }
    }

    private async void OnClearPassportClicked(object sender, EventArgs e)
    {
        App.Audio?.PlayTap();
        await Anim.BouncePress((VisualElement)sender);

        string[] files = _repo.GetSavedFiles();

        if (files.Length == 0 && Passport == null)
        {
            App.Audio?.PlayWarning();
            await DisplayAlert("No Passports", "No saved passports to delete.", "OK");
            return;
        }

        var options = new List<string>();
        for (int i = 0; i < files.Length; i++)
            options.Add(Path.GetFileNameWithoutExtension(files[i]));

        if (Passport != null && !options.Contains(Passport.Profile.Name))
            options.Add($"{Passport.Profile.Name} (current session)");

        string chosen = await DisplayActionSheet("Delete which passport?", "Cancel", null, options.ToArray());

        if (string.IsNullOrEmpty(chosen) || chosen == "Cancel")
            return;

        bool confirm = await DisplayAlert("Confirm Delete",
            $"Are you sure you want to delete {chosen}?",
            "Yes", "No");

        if (!confirm)
            return;

        for (int i = 0; i < files.Length; i++)
        {
            if (Path.GetFileNameWithoutExtension(files[i]) == chosen)
            {
                _repo.Delete(files[i]);

                if (Passport != null && Passport.Profile.Name == chosen)
                {
                    Passport = null;
                    UpdateStatus();
                }

                App.Audio?.PlayDelete();
                await DisplayAlert("Deleted", $"Passport for {chosen} has been deleted.", "OK");
                return;
            }
        }

        if (Passport != null && chosen.StartsWith(Passport.Profile.Name))
        {
            Passport = null;
            UpdateStatus();
            App.Audio?.PlayDelete();
            await DisplayAlert("Cleared", "Current session passport cleared.", "OK");
        }
    }
}
