using System;

using Xamarin.Forms;

#if __IOS__
using UIKit;
#endif

#if __ANDROID__
using Android.App;
using Android.Graphics;
using Android.Content.Res;
#endif

namespace App1
{
	public static class ImageMeter
	{
		public static Size GetImageSize(string fileName)
		{
#if __IOS__
			UIImage image = UIImage.FromFile(fileName);
			return new Size((double)image.Size.Width, (double)image.Size.Height);
#endif

#if __ANDROID__
			var options = new BitmapFactory.Options {
				InJustDecodeBounds = true
			};
			fileName = fileName.Replace('-', '_').Replace(".png", "");
			var resId = Forms.Context.Resources.GetIdentifier(
				fileName, "drawable", Forms.Context.PackageName);
			BitmapFactory.DecodeResource(
				Forms.Context.Resources, resId, options);
			return new Size((double)options.OutWidth, (double)options.OutHeight);
#endif

			return Size.Zero;
		}
	}

	class CenterImageInRelativeLayout
	{
		private static Size ResizeFit(Size originalSize, Size maxSize)
		{
			var widthRatio = maxSize.Width / originalSize.Width;
			var heightRatio = maxSize.Height / originalSize.Height;
			var minAspectRatio = Math.Min(widthRatio, heightRatio);
			return minAspectRatio > 1 ? originalSize : new Size((int)(originalSize.Width * minAspectRatio), (int)(originalSize.Height * minAspectRatio));
		}

		public void ProductSizechange(object sender, EventArgs e)
		{
			//ForceLayout();
			//DisplayAlert("pls show", prodphoto.Width + " " + prodphoto.Height, "kk");
			//var w = (Image)sender.w;
			//var h = ;
		}

		public static RelativeLayout BuildGridElement(Image backgroundImage)
		{
			//var video = ImageSource.FromResource("App1.Images.slider.home-slider-printers.jpg");
			//var video = new Image { Source = ImageSource.FromResource("App1.Images.slider.home-slider-printers.jpg") };
			var referenceLabel = new Label { Opacity = 0 };
			var imgSize = new Size(backgroundImage.Width, backgroundImage.Height);
			var innerLayout = new RelativeLayout { WidthRequest = 1000, HeightRequest = 1000, BackgroundColor = Color.Black };

			//backgroundImage.SizeChanged += ProductSizechange;

			//innerLayout.Children.Add(referenceLabel,
			//	Constraint.RelativeToParent((parent) => parent.Width / 2),
			//	Constraint.RelativeToParent((parent) => parent.Height / 2));

			innerLayout.Children.Add(backgroundImage,
				Constraint.RelativeToParent((parent) =>
				{
					var parentSize = new Size(parent.Width, parent.Height);
					var newImgSize = ResizeFit(imgSize, parentSize);

					return ((double)Decimal.Subtract((decimal)parent.Width, (decimal)newImgSize.Width)) / 2;
				}),
				Constraint.RelativeToParent((parent) =>
				{
					var parentSize = new Size(parent.Width, parent.Height);
					var newImgSize = ResizeFit(imgSize, parentSize);

					return ((double)Decimal.Subtract((decimal)parent.Height, (decimal)newImgSize.Height)) / 2;
				}),
				Constraint.RelativeToParent((parent) =>
				{
					var parentSize = new Size(parent.Width, parent.Height);
					var newPageSize = ResizeFit(imgSize, parentSize);
					return newPageSize.Width;
				}),
				Constraint.RelativeToParent((parent) =>
				{
					var parentSize = new Size(parent.Width, parent.Height);
					var newPageSize = ResizeFit(imgSize, parentSize);
					return newPageSize.Height;
				}));

			//innerLayout.Children.Add(playIcon,
			//	Constraint.RelativeToParent(parent => parent.Width - 40),
			//	Constraint.RelativeToParent(parent => parent.Height / -40));

			//var frame = new Frame
			//{
			//	BackgroundColor = Color.White,
			//	Padding = 10,
			//	Content = innerLayout,
			//	HasShadow = false,
			//	VerticalOptions = LayoutOptions.FillAndExpand
			//};

			//return frame;
			return innerLayout;
		}
	}
}
