﻿<%@ Page Title="Shoreham Sussex Flying Group - G-CGFG Booking" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="BookItems.Site._Default" %>
<%@ Import Namespace="BookItems.Site" %>
<%@ Import Namespace="BookItems.Core" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <br />
    
    <%=UserControlHelper.RenderUserControlToString<NewsListingUserControl>("~/NewsListingUserControl.ascx", nluc => nluc.NewsItemsToDisplay = 2) %>
    
    <br />
    
    <!--
    <table width="100%">
        <tr>
            <td>
                <div align="left">
                    <h2>Booking outside the standard rules</h2>
                    <p>Use the calander on the right to select a day to make a booking in advance of the standard 14 and 15 day rules. Only one such advance booking is allowed per pilot and it must be booked at least 21 days prior to the date.</p>
                </div>
                <asp:Label runat="server" ID="specialBookingLabel" />
            </td>
            <td>
                <div type="text" id="datepicker"></div>
            </td>
        </tr>
    </table>
    -->

    <div id="currenttime">
    	Current Time: <%=DateTime.Now.ToString("HH:mm:ss")%>
    </div>

    <div id="bookingarea">
        <%=UserControlHelper.RenderUserControlToString<BookingPeriodControl>("~/BookingPeriodControl.ascx", null)%>
    </div>
    
    <div id="contactinfo">
		<p>If you are having problems with the booking system or would like something changed please email <a href="mailto:craigcmiller@gmail.com">Craig Miller</a>.</p>
    </div>
    
    <div id="choosePilotDialog" title="Choose Pilot">
    	<p>Choose the pilot:
            <select id="userChoice">
                <% foreach (User user in SessionManager.Shared.BookableItem.Users)
                   {
                       %><option <%=(SessionManager.Shared.User!=null&&user.Id==SessionManager.Shared.User.Id?" selected=\"selected\"":"") %> value="<%=user.Username %>"><%=user.Username %></option><%
                   } %>
            </select>
        </p>
        <button id="choosePilotDilogButtonOK" onclick="confirmOK()">OK</button>
        <button id="choosePilotDilogButtonCancel" onclick="confirmCancel()">Cancel</button>
    </div>

    <div id="deleteBookingDialog" title="Delete Booking">
        <p>Delete the booking for this day for the following pilot: 
            <select id="deleteBookingUserChoice">
                <% foreach (User user in SessionManager.Shared.BookableItem.Users)
                   {
                       %><option <%=(SessionManager.Shared.User!=null&&user.Id==SessionManager.Shared.User.Id?" selected=\"selected\"":"") %> value="<%=user.Username %>"><%=user.Username %></option><%
                   } %>
            </select>
            <button id="deleteBookingDialogDeleteAllForUser" onclick="deleteBookingForUser(document.getElementById('deleteBookingUserChoice').value)">Delete</button>
        </p>

        <hr />
        
        <button id="deleteBookingDialogCancel" onclick="deleteBookingDialogConfirmCancel()">Cancel</button>
    </div>
</asp:Content>