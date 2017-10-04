using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xamarin.Forms;

namespace App1.iOS
{
	public static class ImageMeter
	{
		public static Size GetImageSize(string fileName)
		{
			UIImage image = UIImage.FromFile(fileName);
			return new Size((double)image.Size.Width, (double)image.Size.Height);
		}
	}
}