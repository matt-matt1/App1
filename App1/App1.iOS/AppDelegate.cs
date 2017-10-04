using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

namespace App1.iOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the 
	// User Interface of the application, as well as listening (and optionally responding) to 
	// application events from iOS.
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		//
		// This method is invoked when the application has loaded and is ready to run. In this 
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			App.ScreenWidth = (int)UIScreen.MainScreen.Bounds.Width;
			App.ScreenHeight = (int)UIScreen.MainScreen.Bounds.Height;
			//App.sliderHeight = ;

			global::Xamarin.Forms.Forms.Init();

			string dbPath = FileAccessHelper.GetLocalFilePath("yuma.db3");//https://elearning.xamarin.com/forms/xam160/1-data-storage/exercise1/4-pass-data
			//Settings.DBpath = dbPath;

			Xamarin.FormsMaps.Init();

			FormsPlugin.Iconize.iOS.IconControls.Init();
			Plugin.Iconize.Iconize.With(new Plugin.Iconize.Fonts.FontAwesomeModule());

			LoadApplication(new App(dbPath));

			return base.FinishedLaunching(app, options);
		}
	}
}
