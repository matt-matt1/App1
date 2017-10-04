using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace App1.UWP
{
	public sealed partial class MainPage
	{
		public MainPage()
		{
			this.InitializeComponent();

			Xamarin.FormsMaps.Init("DUx1HdRMp2GwpUbWhZII~aNt9qFj0ryBsJhczkIlSgQ~Alxhd3MlkZpkaQkB2__tnR1mzeh9fvKHhJzmVBBbIpffTZsxrl2pkxu5f0u-hjGO");

			string dbPath = FileAccessHelper.GetLocalFilePath("yuma.db3");//https://elearning.xamarin.com/forms/xam160/1-data-storage/exercise1/4-pass-data
			//Settings.DBpath = dbPath;

			LoadApplication(new App1.App(dbPath));
		}
	}
}
