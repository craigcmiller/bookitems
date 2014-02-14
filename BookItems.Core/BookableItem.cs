using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Iesi.Collections.Generic;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;

namespace BookItems.Core
{
	[ActiveRecord("BookableItems", Lazy = true)]
	public class BookableItem : EntityBase<BookableItem>
	{
		public BookableItem()
		{
		}

		[PrimaryKey]
		public virtual int Id { get; set; }

		[Property]
		public virtual string Name { get; set; }

		[Property]
		public virtual string Description { get; set; }

		[HasAndBelongsToMany(typeof(User), Table = "UserBookableItems", ColumnKey = "BookableItemId", ColumnRef = "UserId", RelationType = RelationType.Set, Lazy = true)]
		public virtual ISet<User> Users { get; set; }

		[HasAndBelongsToMany(typeof(User), Table = "AdminUserBookableItems", ColumnKey = "BookableItemId", ColumnRef = "UserId", RelationType = RelationType.Set, Lazy = true)]
		public virtual ISet<User> Administrators { get; set; }

		[HasMany(typeof(Booking), Inverse = true, Lazy = true)]
		public virtual ISet<Booking> Bookings { get; set; }

		[HasMany(typeof(UserPage), Inverse = true, Lazy = true)]
		public virtual ISet<UserPage> UserPages { get; set; }

		[Property]
		public virtual decimal BookingDayStartTime { get; set; }

		[Property]
		public virtual decimal BookingDayEndTime { get; set; }

		[HasMany(typeof(NewsItem), Inverse = true, Lazy = true)]
		public virtual ISet<NewsItem> NewsItems { get; set; }

        /// <summary>
        /// Gets or sets the total number of shares available for this item
        /// </summary>
        [Property(Default = "30", NotNull = true)]
        public virtual int TotalShares { get; set; }

        /// <summary>
        /// Gets the number of unused shares in this item
        /// </summary>
        public virtual int AvailableShares
        {
            get
            {
                return TotalShares - Users.Count;
            }
        }

		/// <summary>
		/// Creates a booking for the specified user and period
		/// </summary>
		/// <param name="user">User to book for</param>
		/// <param name="start">Start date of the booking</param>
		/// <param name="end">End date of the booking</param>
        /// <param name="isReserve">If true the booking will be made in the reserve slot rather than the main booking slot</param>
		/// <returns>The new booking instance</returns>
		public virtual Booking Book(User user, DateTime start, DateTime end, bool isReserve, out string error)
		{
			// Make sure the booking is valid
			IList<Booking> bookingsToMerge = new List<Booking>();
			error = ValidateBookingAttempt(user, start, end, isReserve, null, bookingsToMerge);
			if (error != null)
				return null;

			if (bookingsToMerge.Count == 0)
			{
				Booking booking = new Booking
				{
					BookedUser = user,
					BookableItem = this,
					StartDate = start,
					EndDate = end,
					IsReserve = isReserve
				};

				if (Bookings == null)
					Bookings = new HashedSet<Booking>();

				Bookings.Add(booking);

				booking.Save();

				return booking;
			}
			else
			{
				DateTime minDateTime = start;
				DateTime maxDateTime = end;

				foreach (Booking booking in bookingsToMerge)
				{
					if (booking.StartDate < minDateTime) minDateTime = booking.StartDate;
					if (booking.EndDate > maxDateTime) maxDateTime = booking.EndDate;
				}

				using (TransactionScope transaction = new TransactionScope())
				{
					try
					{
						bookingsToMerge[0].StartDate = minDateTime;
						bookingsToMerge[0].EndDate = maxDateTime;

						bookingsToMerge[0].Save();

						for (int i = 1; i < bookingsToMerge.Count; i++)
							bookingsToMerge[i].Delete();

						transaction.VoteCommit();
					}
					catch
					{
						transaction.VoteRollBack();
						throw;
					}
				}

				return bookingsToMerge[0];
			}
		}

        /// <summary>
        /// Validation for a booking attempt
        /// </summary>
        /// <remarks>
        /// TODO This code is very purpose specific and will need moved out of here and into some sort of configuration
        /// </remarks>
        /// <param name="user"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="isReserve"></param>
        /// <param name="bookingToIgnore"></param>
        /// <param name="mergeBookings"></param>
		/// <returns>Returns null if booking succeeded otherwise returns the error message</returns>
		private string ValidateBookingAttempt(User user, DateTime start, DateTime end, bool isReserve, Booking bookingToIgnore, IList<Booking> mergeBookings)
		{
			if (start >= end)
				return ("Start of booking must be before the end");

			if ((end - start).Hours > 24)
				return ("Bookings must be 24 hours or less");

			IList<Booking> recentBookings = GetBookingsOneDayAround(start, end, user, isReserve);
			if (bookingToIgnore != null)
				recentBookings.Remove(bookingToIgnore);

			DateTime baseDate = DateTime.Now.Hour < (int)BookingDayStartTime ? DateTime.Today.AddDays(-1) : DateTime.Today;

			DateTime startDateToday = baseDate.AddHours((double)BookingDayStartTime);
			DateTime endDateToday = baseDate.AddHours((double)BookingDayEndTime);

            if (end > endDateToday.AddDays(15))
                return ("Bookings must be within the 14 day rule");

            if (end > DateTime.Today.AddDays(DateTime.Now.Hour >= startDateToday.Hour ? 14 : 13).AddHours((double)BookingDayEndTime + 1))
                return ("You cannot book until 0800 in the morning");
			
			IEnumerable<Booking> invalidBookings = recentBookings.Where (
				                                                booking => ((start - booking.EndDate).TotalHours < 24 + start.Hour && start >= booking.EndDate)
				                                                || ((booking.StartDate - end).TotalHours < 24 + end.Hour && end <= booking.StartDate));

            if (invalidBookings.Count() > 0)
            {
                bool isMerge = false;

                foreach (Booking booking in invalidBookings)
                {
                    if (booking.IsReserve != isReserve) continue;

                    // If we have a merge booking
                    if (booking.StartDate == end || booking.EndDate == start)
                    {
                        isMerge = true;

                        if (booking.Length.TotalHours + (end - start).TotalHours > 24)
                            return ("You cannot extend your booking to over 24 hours");

						string result = ValidateBookingAttempt(user, start > booking.StartDate ? booking.StartDate : start, end < booking.EndDate ? booking.EndDate : end,
                                               isReserve, booking, mergeBookings);
						if (result != null)
							return result;

                        if (!mergeBookings.Contains(booking))
                            mergeBookings.Add(booking);
                    }
                    else if (booking.EndDate == start.Date.AddDays(-1).AddHours((int)BookingDayEndTime + 1))
                    {
                        if (!isReserve)
                        {
                            if (start.Hour != BookingDayStartTime)
                                return ("Overnight bookings must start from the first bookable hour in the morning");

                            if ((end - booking.StartDate).TotalHours > 24)
                                return ("You cannot book over 24 hours");

                            isMerge = true;
                        }
                    }
                }

                if (!isMerge && !isReserve)
                    return ("You already have bookings in a 24 hour vicinity");
            }

            if (end < DateTime.Now)
				return ("Booking in the past is not allowed!");

            // Weekend booking (divided into 3 sections)
            if (start.DayOfWeek == DayOfWeek.Saturday || start.DayOfWeek == DayOfWeek.Sunday)
            {
                // Check the first booking at the start of the day does not end later than 1300
                if (start.Hour == startDateToday.Hour)
                {
                    if (end.Hour > 13)
                        return ("The first weekend booking must end no later than 13:00");
                }
                else
                {
                    // Slots as follows: start of day to 1300, 1300 to 1700, 1700 to end. Max 4 hours
                    if ((end - start).TotalHours > 4.0)
                        return ("Weekend bookings have a maximum length of four hours");

                    if (start.Hour < 13 && end.Hour > 13)
                        return ("The first weekend slot must be between 0900 and 1300");

                    if (start.Hour < 17 && end.Hour > 17)
                        return ("The second weekend slot must be between 1300 and 1700");
                }
            }

			// Check that we are not attempting a double booking
			IList<Booking> allRecentBookings = GetBookingsForDate(start, isReserve);
			if (bookingToIgnore != null) allRecentBookings.Remove(bookingToIgnore);
			foreach (Booking booking in allRecentBookings)
			{
				if (booking.ContainsPartialTimeRange(start, end))
					return ("At least some of this booking time has already been booked");
			}

			return null;
		}

		/// <summary>
		/// Gets a list of all bookings on a specific date. Time is ignored
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public virtual IList<Booking> GetBookingsForDate(DateTime date, bool isReserve)
		{
			DateTime startOfDay = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
			DateTime endOfDay = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);

			SimpleQuery<Booking> bookingsForDate = new SimpleQuery<Booking>(
				"from Booking b where b.StartDate >= :startOfDay and b.EndDate <= :endOfDay and b.IsReserve = :isReserve and b.BookableItem = :bookableItem");
			bookingsForDate.SetParameter("startOfDay", startOfDay);
			bookingsForDate.SetParameter("endOfDay", endOfDay);
			bookingsForDate.SetParameter("isReserve", isReserve);
			bookingsForDate.SetParameter("bookableItem", this);

			return new List<Booking>(bookingsForDate.Execute());
		}

		/// <summary>
		/// Gets an hour indexed dictionary of hours booked in the day with the booking
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public virtual IDictionary<int, Booking> GetHoursBookedForDate(DateTime date, bool isReserve)
		{
			IDictionary<int, Booking> userHours = new Dictionary<int, Booking>();

			foreach (Booking booking in GetBookingsForDate(date, isReserve))
			{
				foreach (int hour in booking.GetHoursBooked())
					userHours.Add(hour, booking);
			}

			return userHours;
		}

		/// <summary>
		/// Gets bookings X days around a date range
		/// TODO Complete days support
		/// </summary>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <param name="user">Pass in null for all users</param>
		/// <returns></returns>
		public virtual IList<Booking> GetBookingsOneDayAround(DateTime start, DateTime end, User user, bool isReserve)
		{
			string queryStr =
				@"from Booking b
				where ((b.EndDate >= :startMinusDay and b.EndDate <= :start) or (b.StartDate <= :endPlusDay and b.StartDate >= :end))
				and BookableItem = :bookableItem and IsReserve = :isReserve"
				+ (user != null ? " and b.BookedUser = :user" : "");

			SimpleQuery<Booking> query = new SimpleQuery<Booking>(queryStr);
			query.SetParameter("startMinusDay", start.AddDays(-1).Date);
			query.SetParameter("start", start);
			query.SetParameter("end", end);
			query.SetParameter("endPlusDay", end.AddDays(2).Date);
			query.SetParameter("bookableItem", this);
            query.SetParameter("isReserve", isReserve);
			if (user != null) query.SetParameter("user", user);

			return new List<Booking>(query.Execute());
		}

        /// <summary>
        /// Gets if this bookable item has <paramref name="user"/> able to book it
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public virtual bool HasUser(User user)
        {
            return Users.Contains(user);
        }

        /// <summary>
        /// Gets if this bookable item has <paramref name="user"/> able to administer it
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public virtual bool HasAdminUser(User user)
        {
            return Administrators.Contains(user);
        }

		/// <summary>
		/// Gets the latest <paramref name="number"/> of news items back from the database
		/// </summary>
		/// <param name="number">Max number of news items to return</param>
		/// <returns>List of news items</returns>
		public virtual IList<NewsItem> GetLatestNewsItems(int number)
		{
			SimpleQuery<NewsItem> query = new SimpleQuery<NewsItem>(
				@"from NewsItem n
				where n.BookableItem = :this
				order by n.CreatedDate desc");

			query.SetParameter("this", this);

			query.SetQueryRange(number);

			return new List<NewsItem>(query.Execute());
		}

		/// <summary>
		/// Gets the latest news items visible on the public site
		/// </summary>
		/// <returns>The latest public news items.</returns>
		/// <param name="number">Number.</param>
		public virtual IList<NewsItem> GetLatestPublicNewsItems(int number)
		{
			SimpleQuery<NewsItem> query = new SimpleQuery<NewsItem>(
				@"from NewsItem n
				where n.BookableItem = :this
				and	n.VisibleOnPublicPage = true
				order by n.CreatedDate desc");
			
			query.SetParameter("this", this);
			
			query.SetQueryRange(number);
			
			return new List<NewsItem>(query.Execute());
		}

		/// <summary>
		/// Creates a news item and saves it to the database
		/// </summary>
		/// <param name="title"></param>
		/// <param name="body"></param>
		/// <param name="postedByUser"></param>
		/// <returns>The new news item instance</returns>
		public virtual NewsItem CreateNewsItem(string title, string body, User postedByUser)
		{
			NewsItem newsItem = new NewsItem
			{
				Title = title,
				BodyText = body,
				CreatedDate = DateTime.Now,
				BookableItem = this,
				PostedByUser = postedByUser,
			};

			newsItem.SaveAndFlush();

			return newsItem;
		}

		/*public virtual void CreateUserPage(User creator, string title)
		{
			UserPage userPage = new UserPage();



			userPage.SaveAndFlush();
		}*/
	}
}
