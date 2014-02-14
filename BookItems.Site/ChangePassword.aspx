<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="BookItems.Site.ChangePassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	
	<br /><br />
	
	<div id="changepasswordarea" align="center">
	    <p>You can change your password here by filling in this simple form.</p>
	    
		<table>
			<tr>
				<td>Current password:</td>
				<td>
					<asp:TextBox runat="server" TextMode="Password" ID="currentPasswordTextBox"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td>New password:</td>
				<td>
					<asp:TextBox runat="server" TextMode="Password" ID="newPasswordTextBox"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td>Confirm password:</td>
				<td>
					<asp:TextBox runat="server" TextMode="Password" ID="confirmPasswordTextBox"></asp:TextBox>
				</td>
			</tr>
			
			<tr>
				<td></td>
				<td><asp:Button ID="changePasswordButton" runat="server" OnClick="changePasswordButton_OnClick" Text="Save"/></td>
			</tr>
		</table>
		
		<asp:Label ID="errorLabel" runat="server" ForeColor="Red" />
	</div>

</asp:Content>
