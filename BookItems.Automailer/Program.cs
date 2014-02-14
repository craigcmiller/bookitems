using System;
using System.Collections.Generic;
using BookItems.Core;
using Castle.ActiveRecord.Framework.Config;
using Castle.ActiveRecord;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace BookItems.Automailer
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			InitActiveRecord ();

			using (SessionScope sessionScope = new SessionScope()) {
				BookableItem bookableItem = BookableItem.FindAll () [0];

				IList<Booking> bookingsToEmail = new List<Booking> ();

				foreach (Booking booking in bookableItem.GetBookingsForDate (DateTime.Today.AddDays (1), false)) {
					if (booking.BookedUser.Email24HoursBeforeBooking)
						bookingsToEmail.Add (booking);
				}

				EmailBookings (bookableItem, bookingsToEmail);
			}
		}

		private static void EmailBookings(BookableItem bookableItem, IList<Booking> bookingsToEmail)
		{
			Console.WriteLine ("Sending emails...");

			SmtpClient smtpClient = new SmtpClient ("smtp.gmail.com", 587) {
				EnableSsl = true,
				UseDefaultCredentials = false,
				Credentials = new NetworkCredential("shorehamsussexflyinggroup@gmail.com", "ssfggcgfg")
			};

			ServicePointManager.ServerCertificateValidationCallback = (s, cert, chain, sslPolicyErrors) => true;

			foreach (Booking booking in bookingsToEmail) {
				Console.WriteLine ("Emailing {0} at {1}", booking.BookedUser.Username, booking.BookedUser.EmailAddress);

				smtpClient.Send (
					"shorehamsussexflyinggroup@gmail.com",
					booking.BookedUser.EmailAddress,
					string.Format ("{0} is booked for tomorrow", bookableItem.Name),
					string.Format ("You have booked {0} between {1} and {2} tomorrow", bookableItem.Name, booking.StartDate.ToShortTimeString (), booking.EndDate.ToShortTimeString ()));
			}
		}

		private static void InitActiveRecord()
		{
			XmlConfigurationSource source = new XmlConfigurationSource(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("BookItems.Automailer.ActiveRecordConfig.xml"));
			source.IsRunningInWebApp = false;
			//source.SetDebugFlag(true);

			ActiveRecordStarter.Initialize(source,
			                               typeof(BookItems.Core.Booking),
			                               typeof(BookItems.Core.BookableItem),
			                               typeof(BookItems.Core.NewsItem),
			                               typeof(BookItems.Core.User));
		}
	}
}
