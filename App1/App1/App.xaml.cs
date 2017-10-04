using App1.Models;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Globalization;

namespace App1
{
	public partial class App : Application
	{
		public static int ScreenWidth;
		public static int ScreenHeight;

		public static SQLiteConnection DB;
		public static string[] DBErrors;

		internal List<SettingsTbl> Settings { get; private set; }

		public App(string dbPath)
		{
			DB = new SQLiteConnection(dbPath);// Settings.DBpath);
											  //LoadSettings();

			this.BindingContext = new MyBindingContext();	//setup binds

			InitializeComponent();

			//MainPage = new App1.MainPage();
			MainPage = new NavigationPage(new Views.Home());
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}

		/// <summary>
		/// event handlers
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void UpdateNetworkInfo(object sender, ConnectivityChangedEventArgs e)
		{
			if (CrossConnectivity.Current != null && CrossConnectivity.Current.ConnectionTypes != null)
			{
				var connectionType = CrossConnectivity.Current.ConnectionTypes.FirstOrDefault();
				//ConnectionDetails.Text = connectionType.ToString();
			}
		}
		void HandleConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
		{
			Type currentPage = this.MainPage.GetType();
			//if (e.IsConnected && currentPage != typeof(NetworkViewPage))
			//    this.MainPage = new NetworkViewPage();
			//else if (!e.IsConnected && currentPage != typeof(NoNetworkPage))
			//    this.MainPage = new NoNetworkPage();
		}
		public void Back_clicked(object sender, EventArgs e)
		{
			//await Application.Current.MainPage.Navigation.PopAsync();
			//Application.Current.MainPage.Navigation.PopAsync();
			App.Current.MainPage.Navigation.PopAsync();
		}
		private void Menu_clicked(object sender, EventArgs e)
		{
			var menuButton = ((Button)sender);
			var menuGrid = ((Grid)menuButton.Parent);
			var containButtons = ((StackLayout)menuGrid.Parent);
			var containAll = ((Grid)containButtons.Parent);
			containAll.Children[2].IsVisible = true;//show overlay
			menuButton.IsVisible = false;//hide
			menuGrid.Children[1].IsVisible = true;//show menuclose
			containAll.Children[3].TranslateTo(0, 0);//open drawer
			var result = DB.CreateTable<MyPage>().ToString();
			var pages = new List<MyPage>();
			if (result=="Created")
			{
				pages = new List<MyPage>
				{
					new MyPage { Title = "Website (English)",            InGrid = true, Row = 1, Column = 1, Icon = "&#xf015;", InMenu = true, TargetEvent = "ENButton_Clicked",         Type = typeof(Views.EN).ToString() },
					new MyPage { Title = "About Us",                     InGrid = true, Row = 1, Column = 3, Icon = "&#xf005;", InMenu = true, TargetEvent = "AboutButton_Clicked",      Type = typeof(Views.About).ToString() },
					new MyPage { Title = "Contact Us",                   InGrid = true, Row = 1, Column = 5, Icon = "&#xf095;", InMenu = true, TargetEvent = "ContactButton_Clicked",    Type = typeof(Views.Contact).ToString() },
					new MyPage { Title = "Services",                     InGrid = true, Row = 1, Column = 7, Icon = "&#xf0ad;", InMenu = true, TargetEvent = "ServicesButton_Clicked",   Type = typeof(Views.Services).ToString() },
					new MyPage { Title = "Website (French-Canadian)",    InGrid = true, Row = 3, Column = 1, Icon = "&#xf024;", InMenu = true, TargetEvent = "QCButton_Clicked",         Type = typeof(Views.QC).ToString() },
					new MyPage { Title = "Laptop Computers",             InGrid = true, Row = 3, Column = 3, Icon = "&#xf109;", InMenu = true, TargetEvent = "LaptopsButton_Clicked",    Type = typeof(Views.Laptops).ToString() },
					new MyPage { Title = "Laser Printers",               InGrid = true, Row = 3, Column = 5, Icon = "&#xf02f;", InMenu = true, TargetEvent = "PrintersButton_Clicked",   Type = typeof(Views.Printers).ToString() },
					new MyPage { Title = "Toner Cartridges",             InGrid = true, Row = 3, Column = 7, Icon = "&#xf150;", InMenu = true, TargetEvent = "TonersButton_Clicked",     Type = typeof(Views.Toners).ToString() }
				};
				//App.DB.Insert(MyPages);
				int inserted = App.DB.InsertAll(pages);
				if (pages.Count != inserted)
				{
					//DisplayAlert(Settings.Error, "Pages could not be inserted", Settings.Accept);
				}
			}
			else
				pages = DB.Table<MyPage>().Where((x) => x.InMenu == true).ToList<MyPage>();
			var list = ((ListView)containAll.Children[3]);
			list.ItemsSource = pages;
			list.ItemSelected += OnSelectPage;
		}
		private void MenuClose_clicked(object sender, EventArgs e)
		{
			var menuButton = ((Button)sender);
			var menuGrid = ((Grid)menuButton.Parent);
			var containButtons = ((StackLayout)menuGrid.Parent);
			var containAll = ((Grid)containButtons.Parent);
			containAll.Children[3].TranslateTo(420, 0);//close drawer
			menuGrid.Children[1].IsVisible = false;//hide menuclose
			menuButton.IsVisible = true;//show
			containAll.Children[2].IsVisible = false;//hide overlay
		}

		/// <summary>
		/// events
		/// </summary>
		void OnSelectPage(Object sender, SelectedItemChangedEventArgs e)
		{
			//if (e.SelectedItem == null || ((ListView)sender).SelectedItem == null)
			//{
			//	return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
			//}
			if (e.SelectedItem != null && ((ListView)sender).SelectedItem != null)
			{
				var list = ((ListView)sender);
				var sel = list.SelectedItem;
				var target = ((MyPage)sel).TargetEvent;
				//var dest = (Page)to;
				list.SelectedItem = null;
				//App.Current.MainPage.Navigation.PushAsync((Page)target());//new Views.Services());
				MenuClose_clicked(this, EventArgs.Empty);
			}
		}


		public void LoadSettings()
		{
			//App.DB.DropTable<SettingsTbl>();
			var result = App.DB.CreateTable<SettingsTbl>().ToString();
			//DisplayAlert(Settings.NoTitle, result, Settings.Accept);
			var MySettings = new List<SettingsTbl>();
			if (result == "Created")
			{
				//Pages.SetDefaultPages();
				MySettings = new List<SettingsTbl>
				{
					new SettingsTbl { Key = "MyLat", Value = "43.74434", Type = "double" },
					new SettingsTbl { Key = "MyLong", Value = "-79.29542", Type = "double" },
					new SettingsTbl { Key = "Address", Value = "24 Hancock Cres, Toronto, ON M1R 2A3" },
					new SettingsTbl { Key = "Phone", Value = "(+1 647) 956-6145" },
					new SettingsTbl { Key = "Email", Value = "sales@yumatechnical.com" },
					new SettingsTbl { Key = "PinLabel", Value = "Home Office" },
					new SettingsTbl { Key = "Subjects", Value = JsonConvert.SerializeObject(new List<string> { "Customer Service", "Webmaster" }), Type = "json" },
					new SettingsTbl { Key = "UseInternetMsg", Value = "" },
					//public static string UseInternetMsg = "We need Internet access for a moment";
					new SettingsTbl { Key = "UseInternetFinishMsg", Value = "" },
					//public static string UseInternetFinishMsg = "We completed Internet useage for this moment";
					new SettingsTbl { Key = "GoogleMapsApi", Value = " AIzaSyBawJKrLXdLhOM1kFKcQiS7YyhhlcTpDbE" },
					new SettingsTbl { Key = "BingBasicUWPKey", Value = "DUx1HdRMp2GwpUbWhZII~aNt9qFj0ryBsJhczkIlSgQ~Alxhd3MlkZpkaQkB2__tnR1mzeh9fvKHhJzmVBBbIpffTZsxrl2pkxu5f0u-hjGO" },
					new SettingsTbl { Key = "BingBasicDevTestKey", Value = "AnucbnJa6mWLbqWhM2daYY -itZKokE2XZJc2AizAIgBYDy0MgEymqZVm7FrZOaRA" },
					new SettingsTbl { Key = "OrderWays", Value = JsonConvert.SerializeObject(new List<string> { "asc", "desc" }), Type = "json" },
					new SettingsTbl { Key = "OrderBy", Value = JsonConvert.SerializeObject(new List<string> { "position" }), Type = "json" },
					new SettingsTbl { Key = "EmailTitle", Value = "Email" },
					new SettingsTbl { Key = "UnableEmail", Value = "Email is disabled" },
					new SettingsTbl { Key = "Accept", Value = "OK" },
					new SettingsTbl { Key = "NotImplemented", Value = "Not implemented yet" },
					new SettingsTbl { Key = "NoTitle", Value = "Message" },
					new SettingsTbl { Key = "EmailMsgPlaceholder", Value = "How can we help?" },
					new SettingsTbl { Key = "EmailMsgMissing", Value = "The Message is not valid" },
					new SettingsTbl { Key = "EmailFromPlaceholder", Value = "your @email.com" },
					new SettingsTbl { Key = "EmailFromMissing", Value = "Your Email Address is not valid" },
					new SettingsTbl { Key = "EmailMsgMinLength", Value = "2", Type = "int" },
					new SettingsTbl { Key = "EmailFromMinLength", Value = "2", Type = "int" },
					//public static string OpenLink = "Will open link in Internet browswer";
					new SettingsTbl { Key = "OpenLink", Value = "" },
					new SettingsTbl { Key = "Yes", Value = "Yes" },
					new SettingsTbl { Key = "No", Value = "No" },
					new SettingsTbl { Key = "Internet", Value = "Internet" },
					new SettingsTbl { Key = "DBpath", Value = "a" },
					new SettingsTbl { Key = "Debug", Value = "9", Type = "int" },
					new SettingsTbl { Key = "PermUseInt", Value = "Do you give permission to load a fresh list from the internet?" },
					new SettingsTbl { Key = "Error", Value = "Error" },
					new SettingsTbl { Key = "NoConn", Value = "No connection" },
					new SettingsTbl { Key = "FailedSave", Value = "Failed to save the feed" },
					new SettingsTbl { Key = "CompletedInt", Value = "Completed the Internet transfer, for now" },
					new SettingsTbl { Key = "ConnFailed", Value = "Connection failed" },
					new SettingsTbl { Key = "UsingLast", Value = "Using last sucessful list" },
					new SettingsTbl { Key = "Cart", Value = "Basket" },
					new SettingsTbl { Key = "Added", Value = " has been added" },
					new SettingsTbl { Key = "Max_tries", Value = "3", Type = "int" },
					new SettingsTbl { Key = "SubjectLabel", Value = "Subject" },
					new SettingsTbl { Key = "FromEmailLabel", Value = "Your Email Address" },
					new SettingsTbl { Key = "MessageLabel", Value = "Message" },
				};
				//foreach (var Setting in MySettings)
				//{
				//	switch (Setting.Type)
				//	{
				//		case "int":
				//			MySettings.Add(new SettingsTbl { Key = Setting.Key, Value = Setting.Value });
				//			break;
				//		case "double":
				//			MySettings.Add(new SettingsTbl { Key = Setting.Key, Value = JsonConvert.SerializeObject(Setting.Value) });
				//			break;
				//		case "list":
				//			MySettings.Add(new SettingsTbl { Key = Setting.Key, Value = JsonConvert.SerializeObject(Setting.Value) });
				//			break;
				//		default:
				//			MySettings.Add(new SettingsTbl { Key = Setting.Key, Value = JsonConvert.SerializeObject(Setting.Value) });
				//			break;
				//	}
				//}
				//App.DB.Insert(MyPages);
				int inserted = App.DB.InsertAll(MySettings);
				if (MySettings.Count != inserted)
				{
					//DisplayAlert(Settings.Error, "Settings could not be inserted", Settings.Accept);
				}
			};
			MySettings = App.DB.Table<SettingsTbl>().ToList<SettingsTbl>();
			foreach(var sett in MySettings)
			{
				if (sett.Type == "json")
				{
					var temp = JsonConvert.DeserializeObject(sett.Value);
					//Settings.object
					//sett.Value = temp;
				}
			}
			Settings = MySettings;
		}
	}

	public class MyBindingContext : INotifyPropertyChanged
	{
		private bool _Backable { get; set; } = true;
		public bool Backable {
			get {
				return _Backable;
			}
			set {
				if (_Backable != value)
				{
					_Backable = value;
					OnPropertyChanged();
				}
			}
		}
		private bool _Overlay { get; set; } = false;
		public bool Overlay {
			get {
				return _Overlay;
			}
			set
			{
				if (_Overlay != value)
				{
					_Overlay = value;
					OnPropertyChanged();
				}
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName] string name = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}
	}
	public class InverseBooleanConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (targetType != typeof(bool))
				throw new InvalidOperationException("The target must be a boolean");
			return !(bool)value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}


	/// <summary>
	/// Adds a local image into XAML source
	/// </summary>
	[ContentProperty("Source")]
	public class ImageResourceExtension : IMarkupExtension
	{
		public string Source { get; set; }
		public object ProvideValue(IServiceProvider serviceProvider)
		{
			if (Source == null)
			{
				return null;
			}
			var imageSource = ImageSource.FromResource(Source);
			return imageSource;
		}
	}
}
