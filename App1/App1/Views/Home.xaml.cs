using App1.Models;
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
	public partial class Home : ContentPage
	{
		//public BindableProperty Backable { get; set; }
		public Home()
		{
			InitializeComponent();

			//Printers_imgslider.GestureRecognizers.Add(new TapGestureRecognizer());
			//sliderStack.HeightRequest = SliderHeight();
			//MainStack.SizeChanged += SetDeviceWidth;
			//Backable = new BindableProperty(false);

			//Title = "fddf";
			var YumaCarousel = new ContentView();
			YumaCarousel.Content = LoadSliderContent();
			Device.StartTimer(new TimeSpan(0, 0, 3), () => { MoveSlider(); return true; }); //repeat every 3 seconds
			Device.StartTimer(new TimeSpan(0, 0, 3), () => { MoveCaption1(); return true; }); //repeat every 3 seconds
			Device.StartTimer(new TimeSpan(0, 0, 3), () => { MoveCaption2(); return true; }); //repeat every 3 seconds
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

		/// <summary>
		/// Set the slider width
		/// </summary>
		public ContentView LoadSliderContent()
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
			foreach (var slide in MySlides)
			{
				var myGrid = new Grid { Margin = 0, Padding = 0 };
				var myImage = new Image { Source = ImageSource.FromResource(slide.ImgSrc) };
				var myImgGest = new TapGestureRecognizer();
				myImgGest.Tapped += (s, e) => {
					DisplayAlert("do action", "Want to go:" + slide.Goto, Settings.Accept);
				};
				myImage.GestureRecognizers.Add(myImgGest);
				myGrid.Children.Add(myImage);
				if (slide.Caption1 != null && slide.Caption1 != "")
				{
					double cap1x, cap1y;
					cap1x = 0.25 * 350;//starting and constant x cord = img.height / 4
					cap1y = 0.75 * 400;//moves from 0 to y cord
					var cap1 = new Label { Text = slide.Caption1, FontSize = 50/*, Style = Resources["Caption1"]*/, Margin = new Thickness(cap1x, 0, 0, 0) };
					myGrid.Children.Add(cap1, 0, 0);
					cap1.TranslateTo(0, cap1y, 2500);
					//cap1.PropertyChanged += ((o2, e2) =>
					//{
					//	cap1.Opacity = cap1.Y / cap1y;
					//});
				}
				if (slide.Caption2 != null && slide.Caption2 != "")
				{
					double cap2x, cap2y;
					cap2x = 0.25 * 350;//moves from 0 to x cord
					cap2y = 0.9 * 400;//starting and constant y cord = img.width / 2
					var cap2 = new Label { Text = slide.Caption2, FontSize = 25/*, Style = Resources["Caption2"]*/, Margin = new Thickness(0, cap2y, 0, 0)/*, TextColor = Resources.Keys["Caption2Color"]*/ };
					myGrid.Children.Add(cap2, 0, 0);
					cap2.TranslateTo(cap2x, 0, 2500);
					//cap2.PropertyChanged += ((o2, e2) =>
					//{
					//	cap2.Opacity = cap2.X / cap2x;
					//});
				}
				//outStack.Children.Add(myGrid);
			}
			ContentView contentView = new ContentView();
			//slide_max = sliderStack.Children.Count;
			//slide_width = Printers_imgslider.Width;
			//List<Slide> Slides = new List<Slide>()
			//{
			//};
			var outSlides = new Grid { Margin = 0, Padding = 0, ColumnSpacing = 0, RowSpacing = 0, WidthRequest = 300 };
			//this.sliderScroll = new ScrollView { Orientation = ScrollOrientation.Horizontal, Margin = 0, Padding = 0 };
			//var outStack = new StackLayout { Spacing = 0, Orientation = StackOrientation.Horizontal, Padding = 0, Margin = 0 };
			//sliderScroll.Content = outStack;
			//outSlides.Children.Add(sliderScroll);
			contentView.Content = outSlides;
			return contentView;
		}


		public void MoveCaption1()//async
		{
			var list = new List<Grid>();// { Printers_txtslider, Toners_txtslider, Laptops_txtslider };
			//if (Printers_txtslider == null)
			//	var Printers_txtslider = this.FindByName<Grid>("Printers_txtslider");
			list.Add(Printers_txtslider);
			//var Toners_txtslider = this.FindByName<Grid>("Toners_txtslider");
			list.Add(Toners_txtslider);
			//var Laptops_txtslider = this.FindByName<Grid>("Laptops_txtslider");
			list.Add(Laptops_txtslider);
			foreach (var cap1 in list)
			{
				if (cap1 != null)
				{
					cap1.Opacity = 0;
					cap1.FadeTo(1, 1500);
					cap1.TranslationY = -200;
					cap1.TranslateTo(0, 0, 3000, Easing.CubicInOut);
				}
			}
		}

		public void MoveCaption2()
		{
			//var Toners_txt2slider = this.FindByName<Grid>("Toners_txt2slider");
			if (Toners_txt2slider != null)
			{
				Toners_txt2slider.Opacity = 0;
				Toners_txt2slider.FadeTo(1, 1500);
				Toners_txt2slider.TranslationX = -100;
				Toners_txt2slider.TranslateTo(0, 0, 3000, Easing.CubicInOut);
			}
		}

		/// <summary>
		/// Shift the image slider to the right(next) (or first) slide
		/// </summary>
		public async void MoveSlider()
		{
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
			//App.DB.DropTable<MyPage>();
			var result = App.DB.CreateTable<MyPage>().ToString();
			//DisplayAlert(Settings.NoTitle, result, Settings.Accept);
			var MyPages = new List<MyPage>();
			if (result == "Created")
			{
				//Pages.SetDefaultPages();
				MyPages = new List<MyPage>
				{
					new MyPage { Title = "Website (English)",            InGrid = true, Row = 1, Column = 1, Icon = "&#xf015;", InMenu = true, TargetEvent = "ENButton_Clicked",         Type = typeof(Views.EN).ToString() },
					new MyPage { Title = "About Us",                     InGrid = true, Row = 1, Column = 3, Icon = "&#xf005;", InMenu = true, TargetEvent = "AboutButton_Clicked",      Type = typeof(Views.About).ToString() },
					new MyPage { Title = "Contact Us",                   InGrid = true, Row = 1, Column = 5, Icon = "&#xf095;", InMenu = true, TargetEvent = "ContactButton_Clicked",    Type = typeof(Views.Contact).ToString() },
					new MyPage { Title = "Services",                     InGrid = true, Row = 1, Column = 7, Icon = "&#xf0ad;", InMenu = true, TargetEvent = "ServicesButton_Clicked",   Type = typeof(Views.Services).ToString() },
					new MyPage { Title = "Website (French-Canadian)",    InGrid = true, Row = 3, Column = 1, Icon = "&#xf024;", InMenu = true, TargetEvent = "QCButton_Clicked",         Type = typeof(Views.QC).ToString() },
					new MyPage { Title = "Laptop Computers",             InGrid = true, Row = 3, Column = 3, Icon = "&#xf109;", InMenu = true, TargetEvent = "LaptopsButton_Clicked",    Type = typeof(Views.Laptops).ToString() },
					new MyPage { Title = "Laser Printers",               InGrid = true, Row = 3, Column = 5, Icon = "&#xf02f;", InMenu = true, TargetEvent = "PrintersButton_Clicked",   Type = typeof(Views.Printers).ToString() },
					new MyPage { Title = "Toner Cartridges",             InGrid = true, Row = 3, Column = 7, Icon = "&#xf150;", InMenu = true, TargetEvent = "TonersButton_Clicked",     Type = typeof(Views.Toners).ToString() }
				};
				//App.DB.Insert(MyPages);
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
			//var GridPages = conn.Table<MyPage>().Where(v => v.InGrid.Equals(true));
			//foreach (var item in GridPages)
			//foreach(var item in MyPages)
			//var app = new App();
			foreach (var item in MyPages)//ERROR:Object reference not set to an instance of an object.!!!!
			{
				var myGrid = new Grid() { Padding = 3, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, BackgroundColor = (Color)App.Current.Resources["YumaPanel"] };
				myGrid.Children.Add(new Label { Text = item.Icon.ToCharArray().ToString().Trim(), Style = (Style)Resources["YumaIconShadow"], FontFamily = "FontAwesome" }, 0, 0);
				myGrid.Children.Add(new Label { Text = item.Icon, Style = (Style)Resources["YumaIcon"] }, 0, 0);
				myGrid.Children.Add(new Label { Text = item.Title, Style = (Style)Resources["YumaLabelShadow"] }, 0, 1);
				myGrid.Children.Add(new Label { Text = item.Title, Style = (Style)Resources["YumaLabel"] }, 0, 1);
				var myButton = new Button { Style = (Style)App.Current.Resources["YumaButton"] };
				//myButton.Clicked += delegate { new Command(() => item.TargetEvent()); };//new EventHandler(new Object(item.TargetEvent), new EventArgs(item.TargetEvent));
				var tap = new TapGestureRecognizer();
				//tap.Tapped += (s, e) => { Command = item.TargetEvent(s, e); };
				//tap.Tapped += (s, e) => { if (item.TargetEvent != null) this.TargetEvent(s, e); };
				//tap.Tapped += (item.TargetEvent as EventHandler);
				myButton.GestureRecognizers.Add(tap);
				myGrid.Children.Add(myButton, 0, 0);
				Grid.SetRowSpan(myButton, 2);
				//GridIcons.Children.Add(myGrid, item.Column, item.Row);
			}
		}

	}
}