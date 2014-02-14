using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using BookItems.Core;

namespace BookItems.Site
{
	public class BookingHandler : IHttpHandler, IRequiresSessionState
	{

		public void ProcessRequest(HttpContext context)
		{
            if (!SessionManager.Shared.IsLoggedIn)
                throw new InvalidOperationException("Invalid action. Your information has been logged.");

			int year = int.Parse(context.Request.Form["year"]);
			int month = int.Parse(context.Request.Form["month"]);
			int date = int.Parse(context.Request.Form["date"]);
			int startHour = context.Request.Form.AllKeys.Contains("startHour") ? int.Parse(context.Request.Form["startHour"]) : 1;
			int endHour = context.Request.Form.AllKeys.Contains("endHour") ? int.Parse(context.Request.Form["endHour"]) : 2;
			bool isReserve = context.Request.Form["type"] == "2";
			bool deleteMode = context.Request.Form["deleteMode"] == "true";

			DateTime startDate = new DateTime(year, month, date, startHour, 0, 0);
			DateTime endDate = new DateTime(year, month, date, endHour + 1, 0, 0);

			string errorMessage = null;

			if (context.Request.Form.AllKeys.Contains("userId"))
			{
				int userId = int.Parse(context.Request.Form["userId"]);

				User user = User.Find(userId);

				if (!string.IsNullOrEmpty(context.Request.Form["deleteMode"]))
				{
					if (context.Request.Form["deleteMode"] == "usertoday")
						Booking.DeleteBookingsForUserOnDate(user, startDate, isReserve);
				}
				else
				{
					SessionManager.Shared.BookableItem.Book(user, startDate, endDate, isReserve, out errorMessage);
				}
			}

			//string bookingDayHtml = RenderPartialToString("~/Views/Shared/BookingDay.ascx", new BookingDayData(startDate, false));
			string bookingDayHtml = UserControlHelper.RenderUserControlToString<BookingDay>("~/BookingDay.ascx", bdControl => bdControl.Data = new BookingDayData(startDate, deleteMode));

			string outputJson = string.Format("({{\n\t\"html\":{0},\n\t\"errorMessage\":{1}\n}})\n",
				JsonSupport.Enquote(bookingDayHtml), JsonSupport.Enquote(errorMessage));

			context.Response.Output.WriteLine(outputJson);
		}

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}
}