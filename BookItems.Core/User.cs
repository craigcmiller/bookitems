using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using Iesi.Collections.Generic;

namespace BookItems.Core
{
	[ActiveRecord("Users", Lazy = true)]
	public class User : EntityBase<User>
	{
		/// <summary>
		/// Gets a user by user name
		/// </summary>
		/// <param name="username"></param>
		/// <returns></returns>
		public static User ByUserName(string username)
		{
			return FindByProperty("Username", username);
		}

		/// <summary>
		/// Gets a user by email address
		/// </summary>
		/// <param name="emailAddress"></param>
		/// <returns></returns>
		public static User ByEmailAddress(string emailAddress)
		{
			return FindByProperty("EmailAddress", emailAddress);
		}

		/// <summary>
		/// Gets a user by either usename or email address
		/// </summary>
		/// <param name="emailAddressOrUsername"></param>
		/// <returns></returns>
		public static User ByEmailAddressOrUsername(string emailAddressOrUsername)
		{
			User user = ByUserName(emailAddressOrUsername);
			if (user == null) return ByEmailAddress(emailAddressOrUsername);

			return user;
		}

		public User()
		{
		}

		[PrimaryKey]
		public virtual int Id { get; set; }

		[Property]
		public virtual string EmailAddress { get; set; }

		[Property]
		public virtual string Username { get; set; }

		[Property]
		public virtual string Password { get; set; }

		[HasAndBelongsToMany(typeof(BookableItem), Table = "UserBookableItems", ColumnKey = "UserId", ColumnRef = "BookableItemId", Inverse = true, RelationType = RelationType.Set, Lazy = true)]
		public virtual ISet<BookableItem> BookableItems { get; set; }

		[HasAndBelongsToMany(typeof(BookableItem), Table = "AdminUserBookableItems", ColumnKey = "UserId", ColumnRef = "BookableItemId", Inverse = true, RelationType = RelationType.Set, Lazy = true)]
		public virtual ISet<BookableItem> AdministeredBookableItems { get; set; }

		[HasMany(typeof(Booking), Inverse = true, Lazy = true)]
		public virtual ISet<Booking> Bookings { get; set; }

        [Property(NotNull = true, Default = "")]
        public virtual string FirstName { get; set; }

        [Property(NotNull = true, Default = "")]
		public virtual string LastName { get; set; }

        [Property(NotNull = true, Default = "")]
        public virtual string MobileNumber { get; set; }

        [Property(NotNull = true, Default = "")]
        public virtual string PhoneNumber { get; set; }

		[HasMany(typeof(NewsItem), Inverse = true, Lazy = true)]
		public virtual ISet<NewsItem> AuthoredNewsItems { get; set; }

        [Property(NotNull = true, Default = "0")]
        public virtual bool EmailNewsPosts { get; set; }

		[Property(NotNull = true, Default = "0")]
		public virtual bool Email24HoursBeforeBooking { get; set; }

		[Property(NotNull = true, Default = "0")]
		public virtual bool CanCreateCustomPages { get; set; }

		[Property(NotNull = true, Default = "0")]
		public virtual bool CanEditCustomPages { get; set; }

		/// <summary>
		/// Gets if this user has a booking outside the standard rules. Only one such booking is allowed
		/// </summary>
		public virtual bool HasSpecialBooking
		{
			get
			{
                return SpecialBooking != null;
			}
		}

        /// <summary>
        /// Gets the single allowed booking beyond the standard rules or returns null if none exists
        /// </summary>
        public virtual Booking SpecialBooking
        {
            get
            {
                IList<Booking> bookings = GetAllBookingsAfter(DateTime.Today.AddDays(16));

                if (bookings.Count == 0) return null;

                return bookings[0];
            }
        }

		/// <summary>
		/// Gets all bookings after <paramref name="dateTime"/> for this user
		/// </summary>
		/// <param name="dateTime"></param>
		/// <returns></returns>
		public virtual IList<Booking> GetAllBookingsAfter(DateTime dateTime)
		{
			SimpleQuery<Booking> bookings = new SimpleQuery<Booking>(
				"from Booking b where b.BookedUser = :thisUser and b.EndDate > :minEndDate");

			bookings.SetParameter("thisUser", this);
			bookings.SetParameter("minEndDate", dateTime);

			return new List<Booking>(bookings.Execute());
		}

		public virtual IList<Booking> GetAllBookingsAfterYesterday()
		{
			return GetAllBookingsAfter(DateTime.Today.AddDays(-1));
		}

        public virtual void ChangePassword(string oldPassword, string newPassword)
        {
            if (Password != oldPassword)
                throw new ArgumentException("Old password incorrect");

            Password = newPassword;

            SaveAndFlush();
        }

		public override bool Equals(object obj)
		{
			User u = obj as User;
			if (u != null)
				return u.Id == this.Id;

			return false;
		}

		public override int GetHashCode()
		{
			return Id;
		}

		public override string ToString()
		{
			return Username;
        }

        #region User management

        /// <summary>
        /// Resets this user to a default profile with <paramref name="password"/>
        /// </summary>
        /// <param name="password"></param>
        /// <param name="defaultDomain">Default domaing for the email address</param>
        public virtual void ResetUser(string password, string defaultDomain)
        {
            EmailAddress = Username.Replace(" ", "").ToLower() + "@" + defaultDomain;
            Password = password;
            PhoneNumber = MobileNumber = "";
            EmailNewsPosts = false;

            SaveAndFlush();
        }

        #endregion
    }
}

