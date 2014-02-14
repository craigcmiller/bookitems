using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using System.Xml.Serialization;

namespace BookItems.Core
{
	[ActiveRecord("Bookings", Lazy = true)]
	public class Booking : EntityBase<Booking>
	{
		private DateTime _createdDate = DateTime.Now;

		public Booking()
		{
		}

		[PrimaryKey]
		public virtual int Id { get; set; }

		[BelongsTo]
		public virtual BookableItem BookableItem { get; set; }

		[BelongsTo]
		public virtual User BookedUser { get; set; }

		[Property]
		public virtual DateTime StartDate { get; set; }

		[Property]
		public virtual DateTime EndDate { get; set; }

		[Property]
		public virtual bool IsReserve { get; set; }

		[Property]
		public virtual DateTime CreatedDate
		{
			get { return _createdDate; }
			set { _createdDate = value; }
		}

		/// <summary>
		/// Gets an in order list of the hours booked
		/// </summary>
		/// <returns></returns>
		public virtual IList<int> GetHoursBooked()
		{
			List<int> hoursBooked = new List<int>();

			for (int i = StartDate.Hour; i < EndDate.Hour; i++)
				hoursBooked.Add(i);

			return hoursBooked;
		}

		/// <summary>
		/// Gets the length of the booking
		/// </summary>
		public virtual TimeSpan Length
		{
			get { return EndDate - StartDate; }
		}

		public virtual int LastHour
		{
			get { return EndDate.Hour; }
		}

		public virtual int FirstHour
		{
			get { return StartDate.Hour; }
		}

		public override bool Equals(object obj)
		{
			Booking booking = obj as Booking;
			if (booking != null)
				return booking.Id == this.Id;
			else
				return false;
		}

		public override int GetHashCode()
		{
			return this.Id;
		}

		/// <summary>
		/// Returns true if this booking contains any time in between <paramref name="start"/> and <paramref name="end"/>
		/// </summary>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <returns></returns>
		public virtual bool ContainsPartialTimeRange(DateTime start, DateTime end)
		{
			return start < EndDate && end > StartDate;
		}

		/// <summary>
		/// Gets all bookings on a certain date
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public static IList<Booking> GetBookingsForDate(DateTime date)
		{
			SimpleQuery<Booking> bookings = new SimpleQuery<Booking>("from Booking b where b.StartDate >= :start and b.EndDate < :end");
			bookings.SetParameter("start", date.Date);
			bookings.SetParameter("end", date.Date.AddDays(1));

			return new List<Booking>(bookings.Execute());
		}

		/// <summary>
		/// Gets all users with a booking on a certain date
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public static IList<User> GetUsersBookedOnDate(DateTime date)
		{
			return GetBookingsForDate(date).Select(booking => booking.BookedUser).Distinct().ToList();
		}

		public static void DeleteBookingsForUserOnDate(User user, DateTime date, bool reserve)
		{
			IList<Booking> bookingsOnDate = GetBookingsForDate(date);

			foreach (Booking booking in bookingsOnDate)
			{
				if (booking.BookedUser.Id == user.Id && booking.IsReserve == reserve)
					booking.DeleteAndFlush();
			}
		}
	}

	public class BookingException : Exception
	{
		public BookingException(string message)
			: base(message)
		{
		}
	}

	public static class BookingCollectionExtensions
	{
		/*public static int[] GetHoursBooked(this IEnumerable<Booking> bookings)
		{
			List<int> bookedHours = new List<int>();
		}*/
	}
}
