using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace App1.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Contact : ContentPage
	{
		public bool FirstTimeAppearing { get; set; }
		public Map Map { get; set; }
		public Position Position { get; set; }
		public MapSpan MapSpan { get; set; }
		public Boolean IsLoading { get; }

		public Contact()
		{
			FirstTimeAppearing = true;
			InitializeComponent();

			SetupForm();	// load text from form fields, etc.
			//tempImgRL = CenterImageInRelativeLayout.BuildGridElement(new Image { Source = "App1.Images.map.jpg" });
			var inner = new Image { Source = ImageSource.FromResource("App1.Images.map.jpg") };
			//tempImgRL.Children.Add(inner,
			//	Constraint.RelativeToParent((parent) => { return (parent.Width - inner.Width) / 2; }),
			//	Constraint.RelativeToParent((parent) => { return (parent.Height - inner.Height) / 2; }),
			//	Constraint.RelativeToParent((parent) => { return parent.Width; }),
			//	Constraint.RelativeToParent((parent) => { return parent.Height; }));
		}

		protected override void OnAppearing()//async
		{
			base.OnAppearing();

			//if (FirstTimeAppearing)
			//{
				FirstTimeAppearing = false;
				var t1 = SetupMap();//await SetupMap();
			//}
			while (!t1.IsCompleted)
			{
				t1.Start();
				t1.Wait();
			}
			stack.Children.Remove(mapStaticImage);
			stack.Children.Insert(1, Map);
		}
		protected override void OnDisappearing()
		{
			base.OnDisappearing();

			//Map.Pins.Clear();
		}

		void SetupForm()
		{
			SubjectLabel.Text = Settings.SubjectLabel;
			FromEmailLabel.Text = Settings.FromEmailLabel;
			FromEmailAddress.Placeholder = Settings.EmailFromPlaceholder;// CrossSettings.Current.GetValueOrDefault("EmailFromPlaceholder", "EmailFromPlaceholder");
			MessageLabel.Text = Settings.MessageLabel;
			Message.Text = Settings.EmailMsgPlaceholder;//.Current.GetValueOrDefault("EmailMsgPlaceholder", "EmailMsgPlaceholder"); //initialize the Editor.Text and TextColor on the XAML file or on the constructor on the code behind with the PlaceHolder or whatever you want.
			Message.TextColor = Color.Gray;
			//subscribe to events
			addressButton.Clicked += OnAddress_clicked;
			emailButton.Clicked += OnEmail_clicked;
			Send.Clicked += OnSend_clicked;
		}

		async Task SetupMap()
		{
			var indicator = new ActivityIndicator { Color = new Color(.5), };
			indicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsLoading");
			indicator.BindingContext = Map;
			//var companies = await Company.GetCarwashesAsync();
			Map = new Map();// CompanyMap(companies);
							//MapView.Children.Add(Map);
			Position = new Position(Settings.MyLat, Settings.MyLong);
			MapSpan = MapSpan.FromCenterAndRadius(Position, Distance.FromKilometers(2.4));
			Map.Pins.Add(new Pin { Label = Settings.PinLabel, Type = PinType.Place, Address = Settings.Address, Position = Position });
			Map.MoveToRegion(MapSpan);
		}

		// event handlers
		public string MySubject = Settings.Subjects[0];
		void OnPickerSelectedIndexChanged(object sender, EventArgs e)
		{
			var picker = (Picker)sender;
			int selectedIndex = picker.SelectedIndex;

			if (selectedIndex != -1)
			{
				MySubject = picker.Items[selectedIndex];
			}
		}
		private void Message_Focused(object sender, FocusEventArgs e) //triggered when the user taps on the Editor to interact with it
		{
			if (Message.Text.Equals(Settings.EmailMsgPlaceholder)) //if you have the placeholder showing, erase it and set the text color to black
			{
				Message.Text = "";
				Message.TextColor = Color.Black;
			}
		}
		private void Message_Unfocused(object sender, FocusEventArgs e) //triggered when the user taps "Done" or outside of the Editor to finish the editing
		{
			if (Message.Text.Equals("")) //if there is text there, leave it, if the user erased everything, put the placeholder Text back and set the TextColor to gray
			{
				Message.Text = Settings.EmailMsgPlaceholder;
				Message.TextColor = Color.Gray;
			}
		}
		void OnEmail_clicked(Object sender, EventArgs e)
		{
			string shareurl = "mailto:" + Settings.Email;
			switch (Device.RuntimePlatform)
			{
				case Device.WinPhone:
					DisplayAlert(Settings.NoTitle,
						Settings.NotImplemented,
						Settings.Accept);
					//DisplayAlert(Settings.NoTitle, Settings.NotImplemented, Settings.Accept);
					break;
				case Device.iOS:
				case Device.macOS:
					var subject = Regex.Replace(Settings.Subjects.ToString(), @"[^\u0000-\u00FF]", string.Empty);
					shareurl += "?subject=" + WebUtility.UrlEncode(subject);
					break;
				default:
					shareurl += "?subject=" + WebUtility.UrlEncode(Settings.Subjects.ToString());
					break;
			}
			Device.OpenUri(new Uri(shareurl));// "mailto:sales@yumatechnical.com?subject=" + Settings.Subjects[0]));
		}
		void OnSend_clicked(Object sender, EventArgs e)
		{
			//var emailMessenger = CrossMessaging.Current.EmailMessenger;
			if (FromEmailAddress.Text == null || FromEmailAddress.Text == "" || FromEmailAddress.Text == Settings.EmailFromPlaceholder
				|| (int)FromEmailAddress.Text?.Length < (int)Settings.EmailFromMinLength)
			{
				FromEmailLabel.TextColor = Color.Red;
				Device.StartTimer(new TimeSpan(0, 0, 1), () => { FromEmailLabel.FadeTo(0.3); return true; }); //repeat every 3 seconds
				FromEmailAddress.Focus();
				DisplayAlert(Settings.EmailTitle, Settings.EmailFromMissing,
					Settings.Accept);
				//break from here
			}
			if (Message.Text == null || Message.Text == "" || Message.Text == Settings.EmailMsgPlaceholder
				|| (int)Message.Text.Length < (int)Settings.EmailMsgMinLength)
			{
				MessageLabel.TextColor = Color.Red;
				Device.StartTimer(new TimeSpan(0, 0, 1), () => { MessageLabel.FadeTo(0.3); return true; }); //repeat every 3 seconds
				Message.Focus();
				DisplayAlert(Settings.EmailTitle,
					Settings.EmailMsgMissing,
					Settings.Accept);
				//break from here
			}
			//if (emailMessenger.CanSendEmail)
			//{
			//	emailMessenger.SendEmail(Settings.Email, MySubject, Message.Text);
			//	//var email = new EmailMessageBuilder()
			//	//  .To(Settings.email)
			//	//  .Cc("cc.plugins@xamarin.com")
			//	//  .Bcc(new[] { "bcc1.plugins@xamarin.com", "bcc2.plugins@xamarin.com" })
			//	//  .Subject(Subject.ToString())
			//	//  .Body(Message.Text)
			//	//  .Build();
			//	//emailMessenger.SendEmail(email);
			//}
			//else
			{
				DisplayAlert(Settings.Internet,
					Settings.UseInternetMsg,
					Settings.Accept);
				//DisplayAlert(Settings.EmailTitle, Settings.UnableEmail, Settings.Accept);
			}
			//string shareurl = "mailto:" + Settings.email;
			//switch (Device.RuntimePlatform)
			//{
			//    case Device.WinPhone:
			//        DisplayAlert("Todo", "Not implemented yet", "OK");
			//        break;
			//    case Device.iOS:
			//    case Device.macOS:
			//        var subject = Regex.Replace(Subject.ToString(), @"[^\u0000-\u00FF]", string.Empty);
			//        var body = Regex.Replace(Message.ToString(), @"[^\u0000-\u00FF]", string.Empty);
			//        var email = Regex.Replace(EmailAddress.ToString(), @"[^\u0000-\u00FF]", string.Empty);
			//        shareurl += "?subject=" + WebUtility.UrlEncode(subject) + "&body=" + WebUtility.UrlEncode(body);
			//        break;
			//    default:
			//        shareurl += "?subject=" + WebUtility.UrlEncode(Subject.ToString()) + "&body=" + WebUtility.UrlEncode(Message.ToString());
			//        break;
			//}
			//Device.OpenUri(new Uri("mailto:sales@yumatechnical.com?subject="+ Subject.ToString()+ "&from="+ EmailAddress.ToString()+ "&body="+ Message.ToString()));
		}
		void OnAddress_clicked(Object sender, EventArgs e)
		{
			switch (Device.RuntimePlatform)
			{
				case Device.WinPhone:
					DisplayAlert(Settings.NoTitle, Settings.NotImplemented,
						Settings.Accept);

					break;
				default:
					Device.OpenUri(new Uri("tel:16479566145"));
					break;
			}
		}
	}
}