/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1
{
	class BindHTML
	{
		public static string GetMyProperty(DependencyObject obj)
		{
			return (string)obj.GetValue(MyPropertyProperty);
		}

		public static void SetMyProperty(DependencyObject obj, string value)
		{
			obj.SetValue(MyPropertyProperty, value);
		}

		// Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty HTMLProperty =
			DependencyProperty.RegisterAttached("HTML", typeof(string), typeof(BindHTML), new PropertyMetadata("", new PropertyChangedCallback(OnHTMLChanged)));
		private static void OnHTMLChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			WebView wv = d as WebView;
			if (wv != null)
			{
				wv.NavigateToString((string)e.NewValue);
			}
		}
	}
}
*/