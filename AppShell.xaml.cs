namespace PassportMD;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute("CreateProfile", typeof(Pages.CreateProfilePage));
		Routing.RegisterRoute("Vitals", typeof(Pages.VitalsPage));
		Routing.RegisterRoute("Summary", typeof(Pages.SummaryPage));
		Routing.RegisterRoute("UpdateWeight", typeof(Pages.UpdateWeightPage));
	}
}
