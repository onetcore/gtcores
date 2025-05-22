namespace Tiktoker;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute("Profile", typeof(DVideoPage));
	}
}
