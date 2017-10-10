using App1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace App1.Views
{
	public class VisualTreeHelper
	{
		public static T GetTemplateChild<T>(Element parent, string name) where T : Element
		{
			if (parent == null)
				return null;
			var templateChild = parent.FindByName<T>(name);
			if (templateChild != null)
				return templateChild;
			foreach (var child in FindVisualChildren<Element>(parent, false))
			{
				templateChild = GetTemplateChild<T>(child, name);
				if (templateChild != null)
					return templateChild;
			}
			return null;
		}
		public static IEnumerable<T> FindVisualChildren<T>(Element element, bool recursive = true) where T : Element
		{
			if (element != null)// && element is Layout)
			{
				var childrenProperty = element.GetType().GetProperty("InternalChildren");//, BindingFlags.Instance | BindingFlags.NonPublic);
				if (childrenProperty != null)
				{
					var children = (IEnumerable<Element>)childrenProperty.GetValue(element);
					foreach (var child in children)
					{
						if (child != null && child is T)
						{
							yield return (T)child;
						}
						if (recursive)
						{
							foreach (T childOfChild in FindVisualChildren<T>(child))
							{
								yield return childOfChild;
							}
						}
					}
				}
			}
		}
	}

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Home : ContentPage
	{
		public ScrollView sliderScroll;
		//public static readonly BindableProperty BackableProperty = BindableProperty.Create(nameof(Backable),
		//						typeof(bool),
		//						typeof(Home),
		//						true,
		//						propertyChanged: OnColorChanged);
		//private static void OnColorChanged(BindableObject bindable, object oldValue, object newValue)
		//{
		//}
		//public bool Backable
		//{
		//	get { return (bool)GetValue(BackableProperty); }
		//	set { SetValue(BackableProperty, value); }
		//}

		public Home()
		{
			App.Backable = false;

			InitializeComponent();
			VisualTreeHelper.GetTemplateChild<Button>(this, "backButton").IsVisible = false;

			//			sliderScroll = this.FindByName<ScrollView>("sliderScroll");
			//sliderStack.HeightRequest = SliderHeight();
			//MainStack.SizeChanged += SetDeviceWidth;
			//Backable = new BindableProperty(false);

			var YumaCarousel = this.FindByName<ContentView>("YumaCarousel");
			if (YumaCarousel != null)
			{
				YumaCarousel.Content = LoadSliderContent();
			}
			MoveCaption1(true);
			Device.StartTimer(new TimeSpan(0, 0, 3), () => { MoveSlider(); return true; }); //repeat every 3 seconds
			Device.StartTimer(new TimeSpan(0, 0, 3), () => { MoveCaption1(); return true; }); //repeat every 3 seconds
			Device.StartTimer(new TimeSpan(0, 0, 3), () => { MoveCaption2(); return true; }); //repeat every 3 seconds

			//App.Current.MainPage.Navigation.NavigationStack. += OnNavigation;
			//if (App.Current.MainPage.Navigation.NavigationStack.Count == 1)
			//{
			//	var herePage = App.Current.MainPage.Navigation.NavigationStack[0];
			//	if (herePage == this)
			//	{
			//		var butt = this.FindByName<Button>("backButton");
			//		if (butt != null)
			//			butt.IsVisible = false;
			//	}
			//}

			LoadPages();
		}

		/// <summary>
		/// Button click EVENTS
		/// </summary>
		private void ContactButton_Clicked(object sender, EventArgs e)
		{
			Application.Current.MainPage.Navigation.PushAsync(new Views.Contact());
		}
		private void AboutButton_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new Views.About());
		}
		private void ServicesButton_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new Views.Services());
		}
		private void QCButton_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new Views.QC());
		}
		private void LaptopsButton_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new Views.Laptops());
		}
		private void PrintersButton_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new Views.Printers());
		}
		private void TonersButton_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new Views.Toners());
		}
		private void ENButton_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new Views.EN());
		}


		/// <summary>
		/// AUTO SLIDER
		/// </summary>
		int slide_max = 3;
		int slide_current = 0;
		double slide_width = 1118;
		//ScrollView sliderScroll = new ScrollView();
		//double[] slide_startX;
/**/
		/// <summary>
		/// Set the slider width
		/// </summary>
		public ContentView LoadSliderContent()
		{
			ContentView contentView = new ContentView();
			this.sliderScroll = new ScrollView { Orientation = ScrollOrientation.Horizontal, Margin = 0, Padding = 0, IsClippedToBounds = true };
			lock (App.collisionLock)
			{
				//App.DB.DropTable<Slide>();
				var result = App.DB.CreateTable<Slide>().ToString();
				//DisplayAlert(Settings.NoTitle, result, Settings.Accept);
				var MySlides = new List<Slide>();
				if (result == "Created")
				{
					MySlides = new List<Slide> {
						new Slide {
							Name ="Printers",
							Caption1 ="Laser Printers",
							Caption2 ="That last",
							Goto =typeof(Views.Printers).ToString(),
							ImgSrc ="App1.Images.slider.home-slider-printers.jpg"
						},//ImageResource 
						new Slide {
							Name ="Toners",
							Caption1 ="Quality Toner Cartridges",
							Goto =typeof(Views.Toners).ToString(),
							ImgSrc ="App1.Images.slider.home-slider-cartridges.jpg"
						},
						new Slide {
							Name="Laptops",
							Caption1 ="Laptop Computers",
							Goto =typeof(Views.Laptops).ToString(),
							ImgSrc ="App1.Images.slider.home-slider-laptops.jpg"
						}
					};
					//App.DB.Insert(Slide);
					int inserted = App.DB.InsertAll(MySlides);
					if (MySlides.Count != inserted)
					{
						DisplayAlert(Settings.Error, "Pages could not be inserted", Settings.Accept);
					}
				}
				else
				{
					MySlides = App.DB.Table<Slide>().ToList<Slide>();
				}
				var outStack = new StackLayout { Spacing = 0, Orientation = StackOrientation.Horizontal, Padding = 0, Margin = 0 };
				//int number = 0;
				foreach (var slide in MySlides)
				{
					var myGrid = new Grid { Margin = 0, Padding = 0, WidthRequest = 1110 };// ColumnDefinitions = 3{ ColumnDefinition, c }, RowDefinitions = 1 };
					var myImage = new Image { Source = ImageSource.FromResource(slide.ImgSrc) };
					var myImgGest = new TapGestureRecognizer();
					var type = Type.GetType(slide.Goto);
					myImgGest.Tapped += (s, e) => { App.Current.MainPage.Navigation.PushAsync(Activator.CreateInstance(type) as Page); };
					myImage.GestureRecognizers.Add(myImgGest);
					myGrid.Children.Add(myImage);//, 0, number++);
					if (slide.Caption1 != null && slide.Caption1 != "")
					{
						double cap1x = 0.25 * 350;//starting and constant x cord = img.height / 4
						//double cap1y = 0.75 * 400;//moves from 0 to y cord
						var inner = new Grid() { Margin = new Thickness(cap1x, 0, 0, 0) };
						inner.Children.Add(new Label { TranslationX = 4, TranslationY = 4, Text = slide.Caption1, Style = (Style)Resources["Caption1Shadow2"] }, 0, 0);
						inner.Children.Add(new Label { TranslationX = 3, TranslationY = 3, Text = slide.Caption1, Style = (Style)Resources["Caption1Shadow"] }, 0, 0);
						inner.Children.Add(new Label { TranslationX = 0, TranslationY = 0, Text = slide.Caption1, Style = (Style)Resources["Caption1Out"] }, 0, 0);
						inner.Children.Add(new Label { TranslationX = 0, TranslationY = 1, Text = slide.Caption1, Style = (Style)Resources["Caption1Out"] }, 0, 0);
						inner.Children.Add(new Label { TranslationX = 0, TranslationY = 2, Text = slide.Caption1, Style = (Style)Resources["Caption1Out"] }, 0, 0);
						inner.Children.Add(new Label { TranslationX = 1, TranslationY = 0, Text = slide.Caption1, Style = (Style)Resources["Caption1Out"] }, 0, 0);
						inner.Children.Add(new Label { TranslationX = 1, TranslationY = 1, Text = slide.Caption1, Style = (Style)Resources["Caption1Out"] }, 0, 0);
						inner.Children.Add(new Label { TranslationX = 1, TranslationY = 2, Text = slide.Caption1, Style = (Style)Resources["Caption1Out"] }, 0, 0);
						inner.Children.Add(new Label { TranslationX = 2, TranslationY = 0, Text = slide.Caption1, Style = (Style)Resources["Caption1Out"] }, 0, 0);
						inner.Children.Add(new Label { TranslationX = 2, TranslationY = 1, Text = slide.Caption1, Style = (Style)Resources["Caption1Out"] }, 0, 0);
						inner.Children.Add(new Label { TranslationX = 2, TranslationY = 2, Text = slide.Caption1, Style = (Style)Resources["Caption1Out"] }, 0, 0);
						inner.Children.Add(new Label { TranslationX = 1, TranslationY = 1, Text = slide.Caption1, Style = (Style)Resources["Caption1"] }, 0, 0);
						//inner.TranslateTo(0, cap1y, 2500);
						//inner.PropertyChanged += ((o2, e2) =>
						//{
						//	inner.Opacity = inner.Y / cap1y;
						//});
						myGrid.Children.Add(inner);
					}
					if (slide.Caption2 != null && slide.Caption2 != "")
					{
						//double cap2x = 0.25 * 350;//moves from 0 to x cord
						//double cap2y = 0.9 * 400;//starting and constant y cord = img.width / 2
						double cap2y = 0.3 * 400;//starting and constant y cord = img.width / 2
						var inner = new Grid() { Margin = new Thickness(0, cap2y, 0, 0) };
						inner.Children.Add(new Label { TranslationX = 4, TranslationY = 4, Text = slide.Caption2, Style = (Style)Resources["Caption2Shadow2"] }, 0, 0);
						inner.Children.Add(new Label { TranslationX = 3, TranslationY = 3, Text = slide.Caption2, Style = (Style)Resources["Caption2Shadow"] }, 0, 0);
						inner.Children.Add(new Label { TranslationX = 0, TranslationY = 0, Text = slide.Caption2, Style = (Style)Resources["Caption2Out"] }, 0, 0);
						inner.Children.Add(new Label { TranslationX = 0, TranslationY = 1, Text = slide.Caption2, Style = (Style)Resources["Caption2Out"] }, 0, 0);
						inner.Children.Add(new Label { TranslationX = 0, TranslationY = 2, Text = slide.Caption2, Style = (Style)Resources["Caption2Out"] }, 0, 0);
						inner.Children.Add(new Label { TranslationX = 1, TranslationY = 0, Text = slide.Caption2, Style = (Style)Resources["Caption2Out"] }, 0, 0);
						inner.Children.Add(new Label { TranslationX = 1, TranslationY = 1, Text = slide.Caption2, Style = (Style)Resources["Caption2Out"] }, 0, 0);
						inner.Children.Add(new Label { TranslationX = 1, TranslationY = 2, Text = slide.Caption2, Style = (Style)Resources["Caption2Out"] }, 0, 0);
						inner.Children.Add(new Label { TranslationX = 2, TranslationY = 0, Text = slide.Caption2, Style = (Style)Resources["Caption2Out"] }, 0, 0);
						inner.Children.Add(new Label { TranslationX = 2, TranslationY = 1, Text = slide.Caption2, Style = (Style)Resources["Caption2Out"] }, 0, 0);
						inner.Children.Add(new Label { TranslationX = 2, TranslationY = 2, Text = slide.Caption2, Style = (Style)Resources["Caption2Out"] }, 0, 0);
						inner.Children.Add(new Label { TranslationX = 1, TranslationY = 1, Text = slide.Caption2, Style = (Style)Resources["Caption2"] }, 0, 0);
						//inner.TranslateTo(cap2x, 0, 2500);
						//inner.PropertyChanged += ((o2, e2) =>
						//{
						//	inner.Opacity = inner.X / cap2x;
						//});
						myGrid.Children.Add(inner);
					}
					outStack.Children.Add(myGrid);
				}
				slide_max = MySlides.Count;//sliderStack.Children.Count;
											//slide_width = Printers_imgslider.Width;
											//List<Slide> Slides = new List<Slide>()
											//{
											//};
				var outSlides = new Grid { Margin = 0, Padding = 0, ColumnSpacing = 0, RowSpacing = 0, WidthRequest = 300 };
				sliderScroll.Content = outStack;
				outSlides.Children.Add(sliderScroll);
				contentView.Content = outSlides;
			}
			return contentView;
		}
/**/

		public async void MoveCaption1(bool first = false)
		{
			var list = new List<Grid>();// { Printers_txtslider, Toners_txtslider, Laptops_txtslider };
			if (this.FindByName<Grid>("Printers_txtslider") != null && this.FindByName<Grid>("Toners_txtslider") != null && this.FindByName<Grid>("Laptops_txtslider") != null)
			{
				//if (Printers_txtslider == null)
				//	var Printers_txtslider = this.FindByName<Grid>("Printers_txtslider");
				//list.Add(Printers_txtslider);
				list.Add(this.FindByName<Grid>("Printers_txtslider"));
				//var Toners_txtslider = this.FindByName<Grid>("Toners_txtslider");
				list.Add(this.FindByName<Grid>("Toners_txtslider"));
				//var Laptops_txtslider = this.FindByName<Grid>("Laptops_txtslider");
				list.Add(this.FindByName<Grid>("Laptops_txtslider"));
			}
			else
			{
				var nextSlide = (slide_current!=(slide_max-1) && !first) ? ((StackLayout)((ScrollView)sliderScroll).Content).Children[slide_current+1] : ((StackLayout)((ScrollView)sliderScroll).Content).Children[0];
				var cap1 = ((Grid)nextSlide).Children[1];
				list.Add((Grid)cap1);
			}
			foreach (var cap1 in list)
			{
				if (cap1 != null)
				{
					cap1.Opacity = 0;
					await cap1.TranslateTo(-100, -200, 1);
					//cap1.TranslationX = -100;
					//cap1.TranslationY = -200;
					var txt = ((Label)cap1.Children[0]).Text;
					var x = cap1.X;
					var y = cap1.Y;
					var tx = cap1.TranslationX;
					var ty = cap1.TranslationY;
					await Task.WhenAll( cap1.FadeTo(1, 1500),
						cap1.TranslateTo(-100, 0, 3000, Easing.CubicInOut) );
				}
			}
		}

		public async void MoveCaption2()
		{
			var Toners_txt2slider = this.FindByName<Grid>("Toners_txt2slider");
			if (Toners_txt2slider != null)
			{
				Toners_txt2slider.Opacity = 0;
				await Task.WhenAll( Toners_txt2slider.FadeTo(1, 1500) );
				Toners_txt2slider.TranslationX = -100;
				await Toners_txt2slider.TranslateTo(0, 0, 3000, Easing.CubicInOut);
			}
			else
			{
				var nextSlide = (slide_current != (slide_max - 1)) ? ((StackLayout)((ScrollView)sliderScroll).Content).Children[slide_current + 1] : ((StackLayout)((ScrollView)sliderScroll).Content).Children[0];
				if (((Grid)nextSlide).Children.Count > 2)
				{
					var cap2 = ((Grid)nextSlide).Children[2];
					cap2.Opacity = 0;
					await cap2.TranslateTo(-100, -200, 1);
					//cap2.TranslationX = -100;
					//cap2.TranslationY = -200;
					await Task.WhenAll( cap2.FadeTo(1, 1500),
						cap2.TranslateTo(0, 400, 3000, Easing.CubicInOut) );
				}
			}
		}

		/// <summary>
		/// Shift the image slider to the right(next) (or first) slide
		/// </summary>
		public async void MoveSlider()
		{
			//var sliderScroll = this.FindByName<ScrollView>("sliderScroll");
			//if (sliderScroll == null)
			//	return;
			if (this.slide_current >= (this.slide_max - 1))
			{
				//move back to start
				this.slide_current = 0;
				await sliderScroll.ScrollToAsync(0, 0, true);
			}
			else
			{
				//move right
				SlideNext_Clicked(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Manual slider events (also use in automatic)
		/// </summary>
		public void SlidePrev_Clicked(object sender, EventArgs e)
		{
			var sliderScroll = this.FindByName<ScrollView>("sliderScroll");
			if (sliderScroll == null)
				return;
			//slide_width = Printers_imgslider.Width;
			this.slide_current -= 1;
			if (this.slide_current < 0)
			{
				this.slide_current = 0;
				sliderScroll.ScrollToAsync(0, 0, true);
			}
			else
				sliderScroll.ScrollToAsync((this.slide_current * slide_width), 0, true);
		}
		public void SlideNext_Clicked(object sender, EventArgs e)
		{
			//var sliderScroll = this.FindByName<ScrollView>("sliderScroll");
			//if (sliderScroll == null)
			//	return;
			//slide_width = Printers_imgslider.Width;
			//sliderScroll.WidthRequest = slide_width;
			this.slide_current += 1;
			if (this.slide_current > (this.slide_max - 1))
				this.slide_current = (this.slide_max - 1);
			else
				sliderScroll.ScrollToAsync((this.slide_current * slide_width), 0, true);
		}

		public void LoadPages()
		{
			lock (App.collisionLock)
			{
				//App.DB.DropTable<MyPage>();
				var result = App.DB.CreateTable<MyPage>().ToString();
				//DisplayAlert(Settings.NoTitle, result, Settings.Accept);
				var MyPages = new List<MyPage>();
				if (result == "Created")
				{
					//Pages.SetDefaultPages();
					MyPages = new List<MyPage>
					{
						new MyPage { Title = "Website (English)",            InGrid = true, Row = 1, Column = 1, Icon = "", Unicode = "&#xf015;", InMenu = true, TargetEvent = "ENButton_Clicked",         Type = typeof(Views.EN).ToString() },
						new MyPage { Title = "About Us",                     InGrid = true, Row = 1, Column = 3, Icon = "", Unicode = "&#xf005;", InMenu = true, TargetEvent = "AboutButton_Clicked",      Type = typeof(Views.About).ToString() },
						new MyPage { Title = "Contact Us",                   InGrid = true, Row = 1, Column = 5, Icon = "", Unicode = "&#xf095;", InMenu = true, TargetEvent = "ContactButton_Clicked",    Type = typeof(Views.Contact).ToString() },
						new MyPage { Title = "Services",                     InGrid = true, Row = 1, Column = 7, Icon = "", Unicode = "&#xf0ad;", InMenu = true, TargetEvent = "ServicesButton_Clicked",   Type = typeof(Views.Services).ToString() },
						new MyPage { Title = "Website (French-Canadian)",    InGrid = true, Row = 3, Column = 1, Icon = "", Unicode = "&#xf024;", InMenu = true, TargetEvent = "QCButton_Clicked",         Type = typeof(Views.QC).ToString() },
						new MyPage { Title = "Laptop Computers",             InGrid = true, Row = 3, Column = 3, Icon = "", Unicode = "&#xf109;", InMenu = true, TargetEvent = "LaptopsButton_Clicked",    Type = typeof(Views.Laptops).ToString() },
						new MyPage { Title = "Laser Printers",               InGrid = true, Row = 3, Column = 5, Icon = "", Unicode = "&#xf02f;", InMenu = true, TargetEvent = "PrintersButton_Clicked",   Type = typeof(Views.Printers).ToString() },
						new MyPage { Title = "Toner Cartridges",             InGrid = true, Row = 3, Column = 7, Icon = "", Unicode = "&#xf150;", InMenu = true, TargetEvent = "TonersButton_Clicked",     Type = typeof(Views.Toners).ToString() }
					};
					int inserted = App.DB.InsertAll(MyPages);
					if (MyPages.Count != inserted)
					{
						DisplayAlert(Settings.Error, "Pages could not be inserted", Settings.Accept);
					}
				}
				else
				{
					MyPages = App.DB.Table<MyPage>().ToList<MyPage>();
				}
				foreach (var item in MyPages)
				{
					var myGrid = new Grid() { Padding = 3, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, BackgroundColor = (Color)App.Current.Resources["YumaPanel"] };
					myGrid.Children.Add(new Label { Text = item.Icon, Style = (Style)Resources["YumaIconShadow"], FontFamily = "FontAwesome" }, 0, 0);
					myGrid.Children.Add(new Label { Text = item.Icon, Style = (Style)Resources["YumaIcon"] }, 0, 0);
					myGrid.Children.Add(new Label { Text = item.Title, Style = (Style)Resources["YumaLabelShadow"] }, 0, 1);
					myGrid.Children.Add(new Label { Text = item.Title, Style = (Style)Resources["YumaLabel"] }, 0, 1);
					var myButton = new Button { Style = (Style)App.Current.Resources["YumaButton"] };
					var tap = new TapGestureRecognizer();
					var pagetype = Type.GetType(item.Type);
					tap.Tapped += (s, e) => { App.Current.MainPage.Navigation.PushAsync(Activator.CreateInstance(pagetype) as Page); };
					myButton.GestureRecognizers.Add(tap);
					myGrid.Children.Add(myButton, 0, 0);
					Grid.SetRowSpan(myButton, 2);
					GridIcons.Children.Add(myGrid, item.Column, item.Row);
				}
			}
		}
	}


	//class MyProperties
	//{
	//	// "HtmlString" attached property for a WebView
	//	public static readonly DependencyProperty HtmlStringProperty =
	//	   DependencyProperty.RegisterAttached("HtmlString", typeof(string), typeof(MyProperties), new PropertyMetadata("", OnHtmlStringChanged));

	//	// Getter and Setter
	//	public static string GetHtmlString(DependencyObject obj) { return (string)obj.GetValue(HtmlStringProperty); }
	//	public static void SetHtmlString(DependencyObject obj, string value) { obj.SetValue(HtmlStringProperty, value); }

	//	// Handler for property changes in the DataContext : set the WebView
	//	private static void OnHtmlStringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	//	{
	//		WebView wv = d as WebView;
	//		if (wv != null)
	//		{
	//			wv.NavigateToString((string)e.NewValue);
	//		}
	//	}
	//}
}