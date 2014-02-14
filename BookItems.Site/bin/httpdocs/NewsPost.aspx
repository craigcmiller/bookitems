<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NewsPost.aspx.cs" Inherits="BookItems.Site.NewsPost" %>
<%@ Import Namespace="BookItems.Site" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<br />
	<br />
	
	<p>News items posted from here will appear at the top of the bookings page.</p>
	
	<div id="newspostarea" align="center">
		<asp:Label ID="errorLabel" runat="server"></asp:Label>
		<table>
			<tr>
				<td>Title:</td>
				<td><asp:TextBox ID="titleTextBox" runat="server" Rows="10"></asp:TextBox></td>
			</tr>
			<tr valign="top">
				<td>Message Body:</td>
				<td><asp:TextBox ID="messageBodyTextBox" runat="server" MaxLength="4000" Rows="10" 
						Columns="40" TextMode="MultiLine"></asp:TextBox></td>
			</tr>
			<tr>
				<td></td>
				<td><asp:Button ID="postNewsButton" runat="server" onclick="postNewsButton_Click"
						Text="Post"></asp:Button></td>
			</tr>
		</table>
	</div>
	
	<br />
	
	<%=UserControlHelper.RenderUserControlToString<NewsListingUserControl>("~/NewsListingUserControl.ascx", nluc => { nluc.NewsItemsToDisplay = 25; nluc.ShowDeleteButton = true; })%>
	
</asp:Content>
