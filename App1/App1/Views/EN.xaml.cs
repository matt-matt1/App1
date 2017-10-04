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
	public partial class EN : ContentPage
	{
		public EN()
		{
			InitializeComponent();
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