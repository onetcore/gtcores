namespace Tiktoker;

public partial class DVideoPage : ContentPage
{
	public DVideoPage()
	{
		InitializeComponent();
	}

	private void OnDownloadButtonClicked(object? sender, EventArgs e)
	{
		var sharedLink = SharedLinkEditor.Text?.Trim();
		if (string.IsNullOrWhiteSpace(sharedLink))
			return;
		DownloadButton.Text = "正在下载视频...";
		DownloadButton.IsEnabled = false;
		Thread.Sleep(2000); // Simulate a delay for downloading
		DownloadButton.Text = "下载视频";
		DownloadButton.IsEnabled = true;
	}
}