using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Web;

namespace Gd;

public partial class DouyinPage : ContentPage
{
	public DouyinPage()
	{
		InitializeComponent();
	}

	private static readonly Regex _regex = new Regex("https://v.douyin.com/(.*)?/");
	private static readonly Regex _location = new Regex("/video/(\\d+)/?");
	private static readonly Regex _data = new Regex(@"window._ROUTER_DATA\s*=\s*(.*?)</script", RegexOptions.Singleline);

	private async void OnSubmitClickedAsync(object sender, EventArgs e)
	{
		var sharedLink = urlEditor.Text.Trim();
		if (sharedLink.Length == 0)
		{
			ShowError("请输入抖音链接");
			return;
		}
		var match = _regex.Match(sharedLink);
		if (!match.Success)
		{
			ShowError("未找到抖音分享链接，清重新输入。");
			return;
		}
		sharedLink = match.Groups[0].Value.Trim();
		var location = await GetLocationAsync(sharedLink);
		if (location == null)
		{
			ShowError("获取地址失败！");
			return;
		}
		match = _location.Match(location);
		if (!match.Success)
		{
			ShowError("获取视频Id失败！");
			return;
		}
		location = $"https://m.douyin.com/share/video/{match.Groups[1].Value}";
		var data = await GetJsonDataAsync(location);
		if (string.IsNullOrWhiteSpace(data))
		{
			ShowError("获取视频数据失败！");
			return;
		}
		data = HttpUtility.UrlDecode(data, Encoding.UTF8);
		var json = JsonSerializer.Deserialize<JsonObject>(data);
		var itemList = json?["loaderData"]?["video_(id)\u002Fpage"]?["videoInfoRes"]?["item_list"]?.AsArray();
		if (itemList == null)
		{
			ShowError("解析视频失败！");
			return;
		}
		var infos = new List<VideoInfo>();
		foreach (var item in itemList)
		{
			infos.Add(new VideoInfo(item!));
		}
		var info = infos.FirstOrDefault();
		if (info == null)
		{
			ShowError("解析视频地址失败！");
			return;
		}
		var video = info.Video.Addresses.Urls[0];
		if (string.IsNullOrWhiteSpace(video))
		{
			ShowError("解析视频地址失败！");
			return;
		}
		video = video.Replace("/playwm/", "/play/");
		var hash = info.Description.IndexOf('#');
		if (hash != -1)
		{
			titleLabel.Text = string.Format("标题：{0}", info.Description[0..hash]);
			descLabel.Text = string.Format("标签：{0}", info.Description[hash..]);
		}
		else
		{
			titleLabel.Text = string.Format("标题: {0}", info.Description);
		}
		videoLabel.Text = string.Format("视频（{1}x{2}）：{0}", video, info.Video.Width, info.Video.Height);
		videoPlayer.MaximumWidthRequest = Width - 40;
		videoPlayer.WidthRequest = info.Video.Width;
		videoPlayer.HeightRequest = info.Video.Height;
		videoPlayer.Source = video;

		// if (args.ContainsKey("o"))
		// {
		//     var outputFile = args["o"] ?? args["y"] ?? $"{info.Id}.mp4";
		//     if (!outputFile.EndsWith(".mp4", StringComparison.OrdinalIgnoreCase))
		//         outputFile = Path.Combine(outputFile, $"{info.Id}.mp4");
		//     var path = args.MapPath(outputFile);
		//     if (args.ContainsKey("y") || !File.Exists(path))//覆盖或者重新下载
		//     {
		//         Cores.InfoLine("正在下载视频...");
		//         await Cores.DownloadAsync(video, path);
		//         Cores.SuccessLine("下载完成 {0}", outputFile);
		//     }
		//     else
		//     {
		//         Cores.SuccessLine("文件已经存在，无需重新下载，完成！");
		//     }
		// }
	}

	private void ShowError(string message)
	{
		DisplayAlert("错误", message, "确定");
		urlEditor.Focus();
	}

	private static async Task<string?> GetJsonDataAsync(string url)
	{
		var handler = new HttpClientHandler { AllowAutoRedirect = false };
		using var client = new HttpClient(handler);
		client.AddIPhone();
		client.DefaultRequestHeaders.Add("Accept", "*/*");
		var html = await client.GetStringAsync(url);
		if (string.IsNullOrWhiteSpace(html))
			return null;
		return _data.Match(html).Groups[1].Value.Trim();
	}

	private static async Task<string?> GetLocationAsync(string sharedLink)
	{
		var handler = new HttpClientHandler { AllowAutoRedirect = false };
		using var client = new HttpClient(handler);
		client.AddIPhone();
		var response = await client.GetAsync(sharedLink);
		if (response.StatusCode == System.Net.HttpStatusCode.Redirect
			&& response.Headers.TryGetValues("Location", out var values))
			return values.FirstOrDefault();
		return null;
	}

	private class VideoInfo
	{
		public VideoInfo(JsonNode item)
		{
			Id = item["aweme_id"]!.ToString();
			Description = item["desc"]!.ToString();
			CreateTime = item["create_time"]!.ToString();
			Author = new Author(item["author"]!);
			Music = new Music(item["music"]!);
			Video = new Video(item["video"]!);
			Statistics = new Statistics(item["statistics"]!);
		}

		/// <summary>
		/// aweme_id。
		/// </summary>
		public string Id { get; }

		/// <summary>
		/// desc。
		/// </summary>
		public string Description { get; }

		/// <summary>
		/// create_time。
		/// </summary>
		public string CreateTime { get; }

		/// <summary>
		/// author
		/// </summary>
		public Author Author { get; }

		/// <summary>
		/// music
		/// </summary>
		public Music Music { get; }

		/// <summary>
		/// video
		/// </summary>
		public Video Video { get; }

		/// <summary>
		/// statistics
		/// </summary>
		public Statistics Statistics { get; }
	}

	private class Author
	{
		public Author(JsonNode json)
		{
			Id = json["short_id"]!.ToString();
			NickName = json["nickname"]!.ToString();
			Signature = json["signature"]!.ToString();
			Thumb = new UrlEntry(json["avatar_thumb"]!);
			Medium = new UrlEntry(json["avatar_medium"]!);
		}

		/// <summary>
		/// short_id。
		/// </summary>
		public string Id { get; }

		/// <summary>
		/// nickname。
		/// </summary>
		public string NickName { get; }

		/// <summary>
		/// signature。
		/// </summary>
		public string Signature { get; }

		/// <summary>
		/// avatar_thumb
		/// </summary>
		public UrlEntry Thumb { get; }

		/// <summary>
		/// avatar_medium
		/// </summary>
		public UrlEntry Medium { get; }
	}

	private class Music
	{
		public Music(JsonNode json)
		{
			Id = json["mid"]!.ToString();
			Title = json["title"]!.ToString();
			Author = json["author"]!.ToString();
			Thumb = new UrlEntry(json["cover_thumb"]!);
			Medium = new UrlEntry(json["cover_medium"]!);
			Hd = new UrlEntry(json["cover_hd"]!);
			Large = new UrlEntry(json["cover_large"]!);
		}

		/// <summary>
		/// mid
		/// </summary>
		public string Id { get; }

		/// <summary>
		/// title
		/// </summary>
		public string Title { get; }

		/// <summary>
		/// author
		/// </summary>
		public string Author { get; }

		/// <summary>
		/// avatar_thumb
		/// </summary>
		public UrlEntry Hd { get; }

		/// <summary>
		/// avatar_medium
		/// </summary>
		public UrlEntry Medium { get; }

		/// <summary>
		/// cover_large
		/// </summary>
		public UrlEntry Large { get; }

		/// <summary>
		/// cover_thumb
		/// </summary>
		public UrlEntry Thumb { get; }
	}

	private class Video
	{
		public Video(JsonNode json)
		{
			Width = (int)json["width"]!.AsValue();
			Height = (int)json["height"]!.AsValue();
			Cover = new UrlEntry(json["cover"]!);
			Addresses = new UrlEntry(json["play_addr"]!);
		}

		/// <summary>
		/// width
		/// </summary>
		public int Width { get; }

		/// <summary>
		/// height
		/// </summary>
		public int Height { get; }

		/// <summary>
		/// cover
		/// </summary>
		public UrlEntry Cover { get; }

		/// <summary>
		/// play_addr
		/// </summary>
		public UrlEntry Addresses { get; }
	}

	private class Statistics
	{
		public Statistics(JsonNode json)
		{
			Id = json["aweme_id"]!.ToString();
			Comment = (int)json["comment_count"]!.AsValue();
			Digg = (int)json["digg_count"]!.AsValue();
			Play = (int)json["play_count"]!.AsValue();
			Share = (int)json["share_count"]!.AsValue();
			Collect = (int)json["collect_count"]!.AsValue();
		}

		/// <summary>
		/// aweme_id
		/// </summary>
		public string Id { get; }

		/// <summary>
		/// comment_count
		/// </summary>
		public int Comment { get; }
		/// <summary>
		/// digg_count
		/// </summary>
		public int Digg { get; }
		/// <summary>
		/// play_count
		/// </summary>
		public int Play { get; }
		/// <summary>
		/// share_count
		/// </summary>
		public int Share { get; }
		/// <summary>
		/// collect_count
		/// </summary>
		public int Collect { get; }
	}

	private class UrlEntry
	{
		public UrlEntry(JsonNode json)
		{
			Uri = json["uri"]!.ToString();
			foreach (var node in json["url_list"]!.AsArray())
			{
				Urls.Add(node!.ToString());
			}
		}

		/// <summary>
		/// uri。
		/// </summary>
		public string Uri { get; }

		/// <summary>
		/// url_list。
		/// </summary>
		public List<string> Urls { get; } = new List<string>();
	}
}