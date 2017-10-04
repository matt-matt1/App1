using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using App1;

namespace App1
{
	//public class Settings : INotifyPropertyChanged
	//{
	//	private static readonly Settings instance = new Settings();
	//	private Settings() { }
	//	public static Settings Instance { get { return instance; } }

	//	public event PropertyChangedEventHandler PropertyChanged;
	//	protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
	//	{
	//		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	//	}

	//	public string LanguageCode { get; set; }

	//	public void LoadFromCsv()
	//	{
	//		//string[][] csv = ;
	//		//lblLoginName = csv[LanguageCode]["lblLoginName"];
	//		//btnLogin = csv[LanguageCode]["btnLogin"];
	//	}

	//public static class Settings
	//{
	//	public static string Str { get; set; }
	//	public static int Int { get; set; }
	//public static List<T> List<T> { get; set; }

	//public double MyLat { get; set; }
	//public double MyLong { get; set; }
	//public string Address { get; set; }
	//public string Phone { get; set; }
	//public string Email { get; set; }
	//public string PinLabel { get; set; }
	//public List<string> Subjects { get; set; }
	//public string UseInternetMsg { get; set; }
	////public string UseInternetMsg  { get; set; }
	//public string UseInternetFinishMsg { get; set; }
	////public string UseInternetFinishMsg  { get; set; }
	//public string GoogleMapsApi { get; set; }
	//public string BingBasicUWPKey { get; set; }
	//public string BingBasicDevTestKey { get; set; }
	//public List<string> OrderWays { get; set; }
	//public List<string> OrderBy { get; set; }
	//public string EmailTitle { get; set; }
	//public string UnableEmail { get; set; }
	//public string Accept { get; set; }
	//public string NotImplemented { get; set; }
	//public string NoTitle { get; set; }
	//public string EmailMsgPlaceholder { get; set; }
	//public string EmailMsgMissing { get; set; }
	//public string EmailFromPlaceholder { get; set; }
	//public string EmailFromMissing { get; set; }
	//public int EmailMsgMinLength { get; set; }
	//public int EmailFromMinLength { get; set; }
	////public string OpenLink { get; set; }
	//public string OpenLink { get; set; }
	//public string Yes { get; set; }
	//public string No { get; set; }
	//public string Internet { get; set; }
	//public string DBpath { get; set; }
	//public int Debug { get; set; }
	//public string PermUseInt { get; set; }
	//public string Error { get; set; }
	//public string NoConn { get; set; }
	//public string FailedSave { get; set; }
	//public string CompletedInt { get; set; }
	//public string ConnFailed { get; set; }
	//public string UsingLast { get; set; }
	//public string Cart { get; set; }
	//public string Added { get; set; }
	//public int Max_tries { get; set; }
	//}
	public static class Settings
	{
		public static double MyLat = 43.74434;
		public static double MyLong = -79.29542;
		public static string Address = "24 Hancock Cres, Toronto, ON M1R 2A3";
		public static string Phone = "(+1 647) 956-6145";
		public static string Email = "sales@yumatechnical.com";
		public static string PinLabel = "Home Office";
		public static List<string> Subjects = new List<string> { "Customer Service", "Webmaster" };
		public static string UseInternetMsg = "";
		//public static string UseInternetMsg = "We need Internet access for a moment";
		public static string UseInternetFinishMsg = "";
		//public static string UseInternetFinishMsg = "We completed Internet useage for this moment";
		public static string GoogleMapsApi = " AIzaSyBawJKrLXdLhOM1kFKcQiS7YyhhlcTpDbE";
		public static string BingBasicUWPKey = "DUx1HdRMp2GwpUbWhZII~aNt9qFj0ryBsJhczkIlSgQ~Alxhd3MlkZpkaQkB2__tnR1mzeh9fvKHhJzmVBBbIpffTZsxrl2pkxu5f0u-hjGO";
		public static string BingBasicDevTestKey = "AnucbnJa6mWLbqWhM2daYY-itZKokE2XZJc2AizAIgBYDy0MgEymqZVm7FrZOaRA";
		public static List<string> OrderWays = new List<string> { "asc", "desc" };
		public static List<string> OrderBy = new List<string> { "position" };
		public static string EmailTitle = "Email";
		public static string UnableEmail = "Email is disabled";
		public static string Accept = "OK";
		public static string NotImplemented = "Not implemented yet";
		public static string NoTitle = "Message";
		public static string EmailMsgPlaceholder = "How can we help?";
		public static string EmailMsgMissing = "The Message is not valid";
		public static string EmailFromPlaceholder = "your@email.com";
		public static string EmailFromMissing = "Your Email Address is not valid";
		public static int EmailMsgMinLength = 2;
		public static int EmailFromMinLength = 2;
		//public static string OpenLink = "Will open link in Internet browswer";
		public static string OpenLink = "";
		public static string Yes = "Yes";
		public static string No = "No";
		public static string Internet = "Internet";
		public static string DBpath = "a";
		public static int Debug = 9;
		public static string PermUseInt = "Do you give permission to load a fresh list from the internet?";
		public static string Error = "Error";
		public static string NoConn = "No connection";
		public static string FailedSave = "Failed to save the feed";
		public static string CompletedInt = "Completed the Internet transfer, for now";
		public static string ConnFailed = "Connection failed";
		public static string UsingLast = "Using last sucessful list";
		public static string Cart = "Basket";
		public static string Added = " has been added";
		public static int Max_tries = 3;
		public static string SubjectLabel = "Subject";
		public static string FromEmailLabel = "Your Email Address";
		public static string MessageLabel = "Message";
	}
}
