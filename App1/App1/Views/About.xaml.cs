using Plugin.Connectivity;
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
	public partial class About : ContentPage
	{
		public About()
		{
			InitializeComponent();

			//int tries = 0;
			int max_tries = ((int)Settings.Max_tries > 0) ? Settings.Max_tries : 3;
			for (var tries = 0; tries < max_tries; tries++)
			{
				if (!CrossConnectivity.Current.IsConnected)
				{
					//var t1 = NoNetwork(tries, max_tries);//.IsCompleted;
					while(!NoNetwork(tries, max_tries).IsCompleted) ;
				}
			}
			//while (!CrossConnectivity.Current.IsConnected && tries < max_tries && NoNetwork(tries++, max_tries).IsCompleted)
			//{
			//	{
			//		while (!NoNetwork(tries, max_tries).IsCompleted) ;
			//	}
			//	//tries++;
			//	//var t1 = NoNetwork(tries, max_tries);
			//	//t1.Wait();
			//}
			//var connWell = CrossConnectivity.Current.IsRemoteReachable("http://yumatechnical.com", 5000);
			//if (CrossConnectivity.Current.IsConnected && connWell.Result)
			if (CrossConnectivity.Current.IsConnected)
			{
				var webView = new WebView { Source = "http://yumatechnical.com/en/content/4-about-us?content_only=1", WidthRequest = 10000, HeightRequest = 10000 };
				webView.Navigated += OnNavigated;
				webView.Navigating += OnNavigating;
				mainGrid.Children.Add(webView, 0, 0);
			}
			//DisplayAlert((Settings.Error != "") ? Settings.Error : "Error", (Settings.Cannot != "") ? Settings.Cannot : "Cannot continue", (Settings.Accept != "") ? Settings.Accept : "OK");
			DisplayAlert((Settings.Error != "") ? Settings.Error : "Error", "Cannot continue", (Settings.Accept != "") ? Settings.Accept : "OK");
			//var t2 = NoNetwork(tries, max_tries);
		}

		private async Task NoNetwork(int cycle, int total)
		{
			var title = (Settings.Error != "") ? Settings.Error : "Error";
			var msg = (Settings.NoConn != "") ? Settings.NoConn : "No connection";
			msg = msg + " (" + ++cycle + "/" + total + ")";
			var ok = (Settings.Accept != "") ? Settings.Accept : "OK";
			await DisplayAlert(title, msg, ok);
		}

		void OnNavigating(object sender, WebNavigatingEventArgs e)
		{
			Loading.IsVisible = true;
		}
		void OnNavigated(object sender, WebNavigatedEventArgs e)
		{
			Loading.IsVisible = false;
		}
	}
}