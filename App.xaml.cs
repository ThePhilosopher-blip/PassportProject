using PassportMD.Services;

namespace PassportMD;

public partial class App : Application
{
	public static AudioService? Audio { get; private set; }

	public App()
	{
		InitializeComponent();
		Audio = new AudioService();
		MainPage = new AppShell();
	}
}
