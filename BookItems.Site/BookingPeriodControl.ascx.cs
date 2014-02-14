using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace BookItems.Site
{
	public partial class BookingPeriodControl : System.Web.UI.UserControl
	{
		public BookingPeriodControl()
		{
			StartDate = DateTime.Today;
			NumberOfDays = 15;
		}

		protected void Page_Load(object sender, EventArgs e)
		{

		}

		/// <summary>
		/// Gets or sets the start date
		/// </summary>
		public DateTime StartDate { get; set; }

		/// <summary>
		/// Gets or sets the number of days to render
		/// </summary>
		public int NumberOfDays { get; set; }

		/// <summary>
		/// Renders the day view
		/// </summary>
		public string RenderStandardDayView()
		{
			StringBuilder html = new StringBuilder();

			for (int i = 0; i < NumberOfDays; i++)
				AppendBookingDay(html, StartDate.AddDays(i));

			return html.ToString();
		}

		private void AppendBookingDay(StringBuilder html, DateTime day)
		{
			html.AppendLine("<div id=\"bookingday" + day.Year + day.Month.ToString().PadLeft(2, '0') + day.Day.ToString().PadLeft(2, '0') + "\">");

			html.AppendLine(UserControlHelper.RenderUserControlToString<BookingDay>("~/BookingDay.ascx", bookingDay => bookingDay.Data = new BookingDayData(day, false)));

			html.AppendLine("</div>");
		}
	}
}