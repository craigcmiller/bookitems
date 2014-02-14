using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using BookItems.Core;

namespace BookItems.Site
{
	/// <summary>
	/// Handler for AJAX requests used to bring back multiple booking days HTML
	/// </summary>
	public class BookingPeriodHandler : IHttpHandler, IRequiresSessionState
	{

		public void ProcessRequest(HttpContext context)
		{
            if (!SessionManager.Shared.IsLoggedIn)
                throw new InvalidOperationException("Invalid action. Your information has been logged.");

			int year = int.Parse(context.Request.Form["year"]);
			int month = int.Parse(context.Request.Form["month"]);
			int day = int.Parse(context.Request.Form["date"]);

			DateTime startDate = new DateTime(year, month, day);

			string errorMessage = "";
			string bookingDayHtml =
				UserControlHelper.RenderUserControlToString<BookingPeriodControl>("~/BookingPeriodControl.ascx", ctrl => { ctrl.NumberOfDays = 2; ctrl.StartDate = startDate; });

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