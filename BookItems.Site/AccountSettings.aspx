<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AccountSettings.aspx.cs" Inherits="BookItems.Site.AccountSettings" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<br /><br />
	
	<div id="accountsettingsarea" align="center">
		<table>
			<tr>
				<td>Email Address:</td>
				<td>
					<asp:TextBox runat="server" ID="emailAddressTextBox"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td>First Name:</td>
				<td>
					<asp:TextBox runat="server" ID="firstNameTextBox"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td>Last Name:</td>
				<td>
					<asp:TextBox runat="server" ID="lastNameTextBox"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td>Phone Number:</td>
				<td>
					<asp:TextBox runat="server" ID="phoneNumberTextBox"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td>Mobile Number:</td>
				<td>
					<asp:TextBox runat="server" ID="mobileNumberTextBox"></asp:TextBox>
				</td>
			</tr>
			<tr>
			    <td>Email Day Before Booking Reminder:</td>
			    <td>
			        <asp:CheckBox runat="server" ID="emailDayBeforeBookingReminder" />
			    </td>
			</tr>
			
			<tr>
				<td></td>
				<td><asp:Button ID="saveButton" runat="server" OnClick="saveButton_OnClick" Text="Save"/></td>
			</tr>
		</table>
	</div>

	<div id="accountsettingsarea" align="center">
		<table>
			<tr><td><a href="ChangePassword.aspx">Change Password</a></td></tr>
		</table>
	</div>
</asp:Content>
