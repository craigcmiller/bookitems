using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using BookItems.Core;

namespace BookItems.Site
{
	public partial class BookingDay : System.Web.UI.UserControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

		public string GenerateBookingSlotsHtml(bool isReserve, DateTime currentDate, int startHour, int endHour, IDictionary<int, Booking> hoursBooked)
		{
			StringBuilder html = new StringBuilder();

            int currentUserId = -1;
            bool alternateColour = false;

            for (int i = startHour; i <= endHour; i++)
            {
				if (currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday) {
					if (i == 13 || i == 17)
						html.Append ("<td class=\"weekendslotspacer\" bgcolor=\"#d1dfe9\"></td>");
				}

                if (hoursBooked.ContainsKey(i))
                {
                    if (currentUserId != hoursBooked[i].BookedUser.Id)
                    {
                        currentUserId = hoursBooked[i].BookedUser.Id;
                        alternateColour = !alternateColour;
                    }

                    html.Append("<td class=\"bookingslotused" + (alternateColour ? "altcolour" : "") + (isReserve ? "reserve" : "") + "\">" + hoursBooked[i].BookedUser.Username + "</td>");
                }
                else
                {
                    html.AppendFormat(
                        "<td id=\"{0}\" class=\"bookingslot" + (isReserve ? "reserve" : "") +
                            "\" onmouseover=\"focusBookableSegment(this)\" onmouseout=\"blurBookableSegment(this)\" onclick=\"clickBookableSegment(this, {1})\"><span></span></td>",
                        currentDate.ToString("yyyyMMdd") + i.ToString().PadLeft(2, '0') + (isReserve ? ".2" : ""),
                        currentDate.Year + ", " + currentDate.Month + ", " + currentDate.Day + ", " + i);
                }
            }

			return html.ToString();
		}

		public string GenerateDeleteBookingSlotsHtml(bool isReserve, DateTime currentDate, int startHour, int endHour, IDictionary<int, Booking> hoursBooked)
		{
			StringBuilder html = new StringBuilder();

			for (int i = startHour; i <= endHour; i++)
			{
				if (hoursBooked.ContainsKey(i))
				{
					bool showEndDeleteButton = false;
					bool showStartDeleteButton = false;

                    html.Append("<td class=\"bookingslotused" + (isReserve ? "reserve" : "") + "\">");

					if (hoursBooked[i].Length.TotalHours > 1)
					{
						if (hoursBooked[i].LastHour - 1 == i)
							showEndDeleteButton = true;
						else if (hoursBooked[i].FirstHour == i)
							showStartDeleteButton = true;

						html.AppendFormat("<button onclick=\"deleteBookingHour({0}, {1}, {2}, {3}, {4}, {5});return false;\">",
							currentDate.Year, currentDate.Month, currentDate.Day, currentDate.Hour, isReserve, showStartDeleteButton);

						if (showEndDeleteButton)
							html.Append("&lt;");
						else if (showStartDeleteButton)
							html.Append("&gt;");

						html.Append("</button>");
					}

					if (!showStartDeleteButton && !showEndDeleteButton)
						html.Append(hoursBooked[i].BookedUser.Username);

					html.Append("</td>");
				}
				else
				{
					html.AppendFormat(
                        "<td id=\"{0}\" class=\"bookingslot" + (isReserve ? "reserve" : "") + "\"><span></span></td>",
						currentDate.ToString("yyyyMMdd") + i.ToString().PadLeft(2, '0') + (isReserve ? ".2" : ""));
				}
			}

			return html.ToString();
		}

		public BookingDayData Data { get; set; }
	}

	public struct BookingDayData
	{
		public readonly DateTime Date;
		public readonly bool DeleteMode;

		public BookingDayData(DateTime date, bool deleteMode)
		{
			this.Date = date;
			this.DeleteMode = deleteMode;
		}
	}
}