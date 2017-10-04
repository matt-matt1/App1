using App1.Class;
using App1.Models;
using Plugin.Connectivity;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Printers : ContentPage
	{
		public string feedUrl = "http://yumatechnical.com/en/module/ps_feeder/rss?id_category=12&orderby=position&orderway=asc";
		public Printers()
		{
			InitializeComponent();

			PutFeedContents();
		}
		/// <summary>
		/// Loads RSS feed and puts into a listview
		/// </summary>
		public List<RSSItem> items;
		//private SQLiteConnection MyDB;

		public async void PutFeedContents() // load and parse RSS feed
		{
			Loading.IsVisible = true;   // set loading on
										//list.IsVisible = false;
			if (await DisplayAlert((Settings.Internet != "") ? Settings.Internet : "Internet",
				(Settings.PermUseInt != "") ? Settings.PermUseInt : "Do you give permission to load a fresh list from the internet?",
				(Settings.Yes != "") ? Settings.Yes : "Yes",
				(Settings.No != "") ? Settings.No : "No"))
			{
				int tries = 0;
				int max_tries = ((int)Settings.Max_tries > 0) ? Settings.Max_tries : 3;
				while (!CrossConnectivity.Current.IsConnected && tries < max_tries)
				{
					//tries++;
					await DisplayAlert((Settings.Error != "") ? Settings.Error : "Error",
						(Settings.NoConn != "") ? Settings.NoConn : "No connection" + " (" + ++tries + "/" + max_tries + ")",
						(Settings.Accept != "") ? Settings.Accept : "OK");
				}
				//var connWell = CrossConnectivity.Current.IsRemoteReachable("http://yumatechnical.com", 5000);
				//if (CrossConnectivity.Current.IsConnected && connWell.Result)
				if (CrossConnectivity.Current.IsConnected)
				{
					try
					{
						var rssfeed = new RSSfeed();    // make instance to use
						string s = await rssfeed.GetHttpStr(feedUrl);   // get feed as string
						this.items = rssfeed.ParseRSS(s); // parse RSS into a list
						int inRSS = this.items.Count;
						//if (ServicesDB..GetTableInfo("services")!="")
						App.DB.DropTable<PrintersTbl>();
						string result = "";
						try
						{
							result = App.DB.CreateTable<PrintersTbl>().ToString();
						}
						catch (SQLiteException ex)
						{
							App.DBErrors[App.DBErrors.Length] = "Create table error:" + ex.Message;
						}
						if (Settings.Debug > 0)
							await DisplayAlert((Settings.NoTitle != "") ? Settings.NoTitle : "NoTitle",
								result, (Settings.Accept != "") ? Settings.Accept : "OK");
						List<PrintersTbl> DBentries = new List<PrintersTbl>();
						foreach (var ritem in items)
						{
							DBentries.Add(new PrintersTbl { Desc = ritem.Desc, Description = ritem.Description, Image = ritem.Image, Link = ritem.Link, MyDesc = ritem.MyDesc, Title = ritem.Title });
						}
						int inDB = App.DB.InsertAll(DBentries);//.ToList<ServicesTbl>()
															   //ServicesDB.InsertAll((List<ServicesTbl>)items);
						if (inDB != inRSS)
							await DisplayAlert((Settings.Error != "") ? Settings.Error : "Error",
								(Settings.FailedSave != "") ? Settings.FailedSave : "Failed to save the feed",
								(Settings.Accept != "") ? Settings.Accept : "OK");
						await DisplayAlert((Settings.Internet != "") ? Settings.Internet : "Internet",
							(Settings.CompletedInt != "") ? Settings.CompletedInt : "Completed the Internet transfer, for now",
							(Settings.Accept != "") ? Settings.Accept : "OK");
					}
					catch (Exception ex)
					{
						await DisplayAlert((Settings.NoTitle != "") ? Settings.NoTitle : "NoTitle", ex.Message, (Settings.Accept != "") ? Settings.Accept : "OK");
					}
				}
				else
				{
					string msg = (Settings.ConnFailed != "") ? Settings.ConnFailed : "Connection failed";
					string msg2 = (Settings.UsingLast != "") ? Settings.UsingLast : "Using last sucessful list";
					await DisplayAlert((Settings.Internet != "") ? Settings.Internet : "Internet",
						msg + ". " + msg2,
						(Settings.Accept != "") ? Settings.Accept : "OK");
					//this.items = App.DB.Table<ServicesTbl>();//.ToList();
					//var myItems = App.DB.Table<ServicesTbl>();//.ToList();
					//foreach(var item in myItems)
					//{
					//	this.items.Add(new RSSItem { Title = item.Title, Desc = item.Desc, Description = item.Description, Image = item.Image, Link = item.Link, ID = item.ID, MyDesc = item.MyDesc });
					//}
				}
			}
			//else
			{
				var myItems = App.DB.Table<PrintersTbl>();
				foreach (var item in myItems)
				{
					var htmlSource = new HtmlWebViewSource
					{
						Html = item.Desc
					};
					this.items.Add(new RSSItem
					{
						Title = item.Title,
						Description = item.Description,
						Image = item.Image,
						Link = item.Link,
						ID = item.Id,
						MyDesc = item.MyDesc,
						Desc = htmlSource.Html
					});
				}
				//this.items = App.DB.Table<ServicesTbl>();//.ToList();
			}
			this.list.ItemsSource = this.items;   // put in listview
			Loading.IsVisible = false;  // set loading off
			list.IsVisible = true;
			list.ItemSelected += OnSelectRSS; // link tap event to handler

			var moreAction = new MenuItem { Text = "Add to Basket" };
			moreAction.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));
			moreAction.Clicked += AddToCart;
			//ContextActions.Add(moreAction);
		}

		/// <summary>
		/// events
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void OnSelectRSS(Object sender, SelectedItemChangedEventArgs e)
		{
			//if (e.SelectedItem == null || ((ListView)sender).SelectedItem == null)
			//{
			//	return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
			//}
			if (e.SelectedItem != null && ((ListView)sender).SelectedItem != null)
			{
				((ListView)sender).SelectedItem = null;
				var link = this.items[this.items.IndexOf((RSSItem)e.SelectedItem)].Link;
				if (Settings.OpenLink != "")
				{
					if (DisplayAlert((Settings.Internet != "") ? Settings.Internet : "Internet", (Settings.OpenLink != "") ? Settings.OpenLink : "OpenLink",
						(Settings.Yes != "") ? Settings.Yes : "Yes",
						(Settings.No != "") ? Settings.No : "No").Result == false)
						return;
				}
				Device.OpenUri(new Uri(link));
			}
		}
		public void AddToCart(object sender, EventArgs e)
		{
			var mi = ((MenuItem)sender);
			string msg = (Settings.Added != "") ? Settings.Added : " has been added";
			DisplayAlert((Settings.Cart != "") ? Settings.Cart : "Basket", mi.CommandParameter + msg, (Settings.Accept != "") ? Settings.Accept : "OK");//temp
		}
	}
}