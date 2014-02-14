<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Help.aspx.cs" Inherits="BookItems.Site.Help" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	
	<br />
	<br />
	
	<h2>Help</h2>
	
	<div id="bookinghelp">
		
		<div class="bookinghelpitem">
			<h3>Booking a slot</h3>
			<ol>
				<li>Click on the first hour that you wish to book. This should go green.</li>
				<li>Click on the last hour that you wish to book. This will display a pilot choice.</li>
				<li>Select the pilot (if it is someone other than the selected pilot.)</li>
				<li>Click OK to save your booking. Any problems will be displayed immediately.</li>
			</ol>
		</div>
		
	</div>
	
</asp:Content>
