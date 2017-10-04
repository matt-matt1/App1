using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace App1.Class
{
	[XmlRoot("channel")]
	public class ItemList
	{
		public ItemList() { Items = new List<RSSItem>(); }
		[XmlElement("item")]
		public List<RSSItem> Items { get; set; }
	}
	/// <summary>
	/// Each item in the RSS feed
	/// </summary>
	public class RSSItem
	{
		[XmlElement("title")]
		public string Title { get; set; }
		[XmlElement("description")]
		public string Description { get; set; }
		public string Desc { get; set; }
		[XmlElement("link")]
		public string Link { get; set; }
		[XmlIgnore]
		public int ID { get; set; }
		[XmlIgnore]
		public string Image { get; set; }
		[XmlIgnore]
		public string MyDesc { get; set; }
	}

	class RSSfeed
	{
		void TestConnection()
		{
			var connected = CrossConnectivity.Current.IsConnected;
		}
		/// <summary>
		/// Sends a HTTP request and return the response
		/// </summary>
		/// <param name="feedUrl"></param>
		/// <param name="GetStatusCode"></param>
		/// <param name="headers"></param>
		/// <returns>HTTP response</returns>
		public async Task<string> GetHttpStr(string feedUrl, bool GetStatusCode = true, string[] headers = null)
		{
			var client = new HttpClient();
			string result;
			if (Settings.UseInternetMsg != "")
				await App.Current.MainPage.DisplayAlert(Settings.Internet, Settings.UseInternetMsg, Settings.Accept);
			if (GetStatusCode)
			{
				if (headers != null && headers[0] != "")
				{
					foreach (string header in headers)
					{
						string[] words = header.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
						client.DefaultRequestHeaders.Add(words[0].Trim(), words[1].Trim());
					}
				}
				HttpResponseMessage response = await client.GetAsync(feedUrl);
				if (!response.IsSuccessStatusCode)
				{
					await App.Current.MainPage.DisplayAlert(
						"Failed to get web response", "ERROR: " + response.StatusCode.ToString() + " (" + response.ReasonPhrase.ToString() + ")", Settings.Accept);
				}
				HttpContent content = response.Content;
				result = await content.ReadAsStringAsync();
			}
			else
			{
				result = await client.GetStringAsync(feedUrl);
			}
			if (Settings.UseInternetFinishMsg != "")
				await App.Current.MainPage.DisplayAlert(Settings.Internet, Settings.UseInternetFinishMsg, Settings.Accept);
			return result;
		}
		/// <summary>
		/// Divide a string into RSSItem elements
		/// </summary>
		/// <param name="rss"></param>
		/// <returns>Array of RSSItems</returns>
		public List<RSSItem> ParseRSS(string rss)
		{
			XDocument doc = XDocument.Parse(rss);
			int id = 0;
			return (from item in doc.Element("rss").Element("channel").Elements("item")
					select new RSSItem
					{
						Title = item.Element("title").Value.Trim(),
						Link = item.Element("link").Value.Trim(),
						Description = item.Element("description").Value.Trim(),
						Desc = item.Element("description").Value.Trim(),//.Replace(Regex.Match(item.Element("description").Value.Trim(), @"<img[^>]*>").Groups[0].Value.Trim(), ""),//item.Element("description").Value.Trim(),
																		//Image = Regex.Match(item.Element("description").Value, @"<img[^>]*>").Groups[0].Value.Trim(),
						ID = id++
					}).ToList();
		}
	}
}
