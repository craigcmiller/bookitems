<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BookingDay.ascx.cs" Inherits="BookItems.Site.BookingDay" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="BookItems.Core" %>
<%@ Import Namespace="BookItems.Site" %>

<% 
   DateTime currentDate = Data.Date;
   BookableItem bookableItem = SessionManager.Shared.BookableItem;
   IDictionary<int, Booking> hoursBooked = bookableItem.GetHoursBookedForDate(currentDate, false);
   IDictionary<int, Booking> reserveHoursBooked = bookableItem.GetHoursBookedForDate(currentDate, true);
   int startHour = (int)bookableItem.BookingDayStartTime, endHour = (int)bookableItem.BookingDayEndTime;
    %>

<div class="bookingday">
    <h3><%=currentDate.ToString("ddd dd/MM/yyyy") %></h3>
    <table class="bookingdaytable">
        <tr>
            <td>Time</td>
            
            <%  for (int i = startHour; i <= endHour; i++)
                {
                	if (currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday) {
						if (i == 13 || i == 17)
							%><td class="bookingslottime" bgcolor="#d1dfe9"></td><%
					}
                   %><td class="bookingslottime">
                    <%=i.ToString().PadLeft(2, '0') + ":00"%>
                   </td><%
                } %>
        </tr>
        <tr>
            <td>Booking</td>

            <%if (Data.DeleteMode) Response.Output.WriteLine(GenerateDeleteBookingSlotsHtml(false, currentDate, startHour, endHour, hoursBooked));
              else Response.Output.WriteLine(GenerateBookingSlotsHtml(false, currentDate, startHour, endHour, hoursBooked));%>

            <td>
                <button onclick="showDeleteBookingDialog(false, <%=currentDate.Year+", "+currentDate.Month+", "+currentDate.Day %>);return false;">Delete Booking</button>
            </td>
        </tr>
        <tr>
            <td>Reserve</td>

            <%if (Data.DeleteMode) Response.Output.WriteLine(GenerateDeleteBookingSlotsHtml(true, currentDate, startHour, endHour, reserveHoursBooked));
              else Response.Output.WriteLine(GenerateBookingSlotsHtml(true, currentDate, startHour, endHour, reserveHoursBooked));%>

            <td>
                <button onclick="showDeleteBookingDialog(true, <%=currentDate.Year+", "+currentDate.Month+", "+currentDate.Day %>);return false;">Delete Reserve</button>
            </td>
        </tr>
    </table>
</div>

