using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace App1.Droid
{
	[Activity(Label = "App1", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			App.ScreenWidth = (int)(Resources.DisplayMetrics.WidthPixels / Resources.DisplayMetrics.Density);
			App.ScreenHeight = (int)(Resources.DisplayMetrics.HeightPixels / Resources.DisplayMetrics.Density);
			//App.sliderHeight = ;

			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(bundle);

			global::Xamarin.Forms.Forms.Init(this, bundle);

			Xamarin.FormsMaps.Init(this, bundle);

			string dbPath = FileAccessHelper.GetLocalFilePath("yuma.db3");//https://elearning.xamarin.com/forms/xam160/1-data-storage/exercise1/4-pass-data
			//Settings.DBpath = dbPath;

			//FormsPlugin.Iconize.Droid.IconControls.Init(Resource.Id.toolbar);
			//Plugin.Iconize.Iconize.With(new Plugin.Iconize.Fonts.FontAwesomeModule());

			LoadApplication(new App(dbPath));
		}
	}
}

