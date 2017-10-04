using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Android.Graphics;
using Android.Content.Res;

namespace App1.Droid
{
	public static class ImageMeter
	{
		public static Size GetImageSize(string fileName)
		{
			var options = new BitmapFactory.Options
			{
				InJustDecodeBounds = true
			};
			fileName = fileName.Replace('-', '_').Replace(".png", "").Replace(".jpg", "");
			var resField = typeof(Resource.Drawable).GetField(fileName);
			var resId = (int)resField.GetValue(null);
			//var resId = Forms.Context.Resources.GetIdentifier(fileName, "drawable", Forms.Context.PackageName);
			BitmapFactory.DecodeResource(Forms.Context.Resources, resId, options);
			return new Size((double)options.OutWidth, (double)options.OutHeight);
		}
	}
}