using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();

			App.Current.MainPage = new NavigationPage(new Views.Home());
		}
	}
}
