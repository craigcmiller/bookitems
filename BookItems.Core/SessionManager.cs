using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Iesi.Collections.Generic;

namespace BookItems.Core
{
	public class SessionManager
	{
		private static readonly SessionManager s_sharedSessionManager;
        private static readonly int s_bookableItemId;

		static SessionManager()
		{
			s_sharedSessionManager = new SessionManager();
			BookableItem bookableItem = BookableItem.FindFirst();
            if (bookableItem == null)
            {
                InsertSsfgData();
                bookableItem = BookableItem.FindFirst();
            }
            s_bookableItemId = bookableItem.Id;
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
                BookingDayEndTime = 19,
                TotalShares = 30
            };

			for (int i = 0; i < users.Length; i++)
			{
				User user = users[i];

                user.FirstName = user.LastName = user.PhoneNumber = user.MobileNumber = "";

				user.Password = "ssfggcgfg";
				user.BookableItems = new HashedSet<BookableItem>();
				if (i != 30)
				{
					bookableItem.Users.Add(user);
					user.BookableItems.Add(bookableItem);
				}

				if (i == 20 || i == 30)
				{
                    user.AdministeredBookableItems = new HashedSet<BookableItem> { bookableItem };

				    bookableItem.Administrators.Add(user);
				}

				user.Save();
			}

			bookableItem.Save();
		}

		/// <summary>
		/// Gets the shared session info instance
		/// </summary>
		public static SessionManager Shared
		{
			get { return s_sharedSessionManager; }
		}

		private SessionManager()
		{
		}

		/// <summary>
		/// Attempts to login and returns true on success
		/// </summary>
		/// <param name="usernameOrEmailAddress"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		public bool AttemptLogin(string usernameOrEmailAddress, string password)
		{
			User user = User.ByEmailAddressOrUsername(usernameOrEmailAddress);
			if (user != null && user.Password.ToLower() == password.ToLower())
			{
				User = user;
				return true;
			}

			return false;
		}

		public void Logout()
		{
			User = null;
		}

		public User User
		{
			private set
			{
                HttpContext.Current.Session["UserId"] = value.Id;
			}
			get
			{
			    if (HttpContext.Current.Session["UserId"] != null)
                    return User.Find(HttpContext.Current.Session["UserId"]);

                return null;
			}
		}

		public bool IsLoggedIn
		{
			get { return User != null; }
		}

		public BookableItem BookableItem
		{
			get
			{
                return BookableItem.Find(s_bookableItemId);
			}
		}
	}
}
