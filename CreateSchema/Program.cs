using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework.Config;
using Iesi.Collections.Generic;
using BookItems.Core;

namespace CreateSchema
{
	class Program
	{
		static void Main(string[] args)
		{
			XmlConfigurationSource source = new XmlConfigurationSource("appconfig.xml");
			source.SetDebugFlag(true);


			ActiveRecordStarter.Initialize(System.Reflection.Assembly.GetAssembly(typeof(BookItems.Core.UserPage)), source);


			//User[] users = User.FindAll();

			//InsertTestData();

			ActiveRecordStarter.UpdateSchema();
			//ActiveRecordStarter.CreateSchema();

			//InsertSsfgData();

			Console.WriteLine("Done...");
			Console.ReadLine();
		}

		private static void InsertSsfgData()
		{
			User[] users =
			{
				new User { Username = "1", EmailAddress = "1@com.com" },
				new User { Username = "2", EmailAddress = "2@com.com" },
				new User { Username = "3", EmailAddress = "3@com.com" },
				new User { Username = "4", EmailAddress = "4@com.com" },
				new User { Username = "5", EmailAddress = "5@com.com" },
				new User { Username = "6", EmailAddress = "6@com.com" },
				new User { Username = "7", EmailAddress = "7@com.com" },
				new User { Username = "8", EmailAddress = "8@com.com" },
				new User { Username = "9", EmailAddress = "9@com.com" },
				new User { Username = "10", EmailAddress = "10@com.com" },
				new User { Username = "11", EmailAddress = "11@com.com" },
				new User { Username = "12", EmailAddress = "12@com.com" },
				new User { Username = "13", EmailAddress = "13@com.com" },
				new User { Username = "14", EmailAddress = "14@com.com" },
				new User { Username = "15", EmailAddress = "15@com.com" },
				new User { Username = "16", EmailAddress = "16@com.com" },
				new User { Username = "17", EmailAddress = "17@com.com" },
				new User { Username = "18", EmailAddress = "18@com.com" },
				new User { Username = "19", EmailAddress = "19@com.com" },
				new User { Username = "20", EmailAddress = "20@com.com" },
				new User { Username = "21", EmailAddress = "21@com.com" },
				new User { Username = "22", EmailAddress = "22@com.com" },
				new User { Username = "23", EmailAddress = "23@com.com" },
				new User { Username = "24", EmailAddress = "24@com.com" },
				new User { Username = "25", EmailAddress = "25@com.com" },
				new User { Username = "26", EmailAddress = "26@com.com" },
				new User { Username = "27", EmailAddress = "27@com.com" },
				new User { Username = "28", EmailAddress = "28@com.com" },
				new User { Username = "29", EmailAddress = "29@com.com" },
				new User { Username = "30", EmailAddress = "30@com.com" },
				new User { Username = "airport", EmailAddress = "airport" }
			};

			BookableItem bookableItem = new BookableItem
			{
				Name = "G-CGFG",
				Description = "Shoreham Sussex Flying Group (SSFG) Cessna 152 - G-CGFG",
				Users = new HashedSet<User>(),
				Administrators = new HashedSet<User>(),
				BookingDayStartTime = 8,
				BookingDayEndTime = 19
			};

			for (int i = 0; i < users.Length; i++)
			{
				User user = users[i];

				user.Password = "password";
				user.BookableItems = new HashedSet<BookableItem>();
				if (i != 30)
				{
					bookableItem.Users.Add(user);
					user.BookableItems.Add(bookableItem);
				}

				if (i == 20 || i == 30)
				{
					user.AdministeredBookableItems = new HashedSet<BookableItem>();
					user.AdministeredBookableItems.Add(bookableItem);

					bookableItem.Administrators.Add(user);
				}

				user.Save();
			}

			bookableItem.Save();
		}
	}
}
