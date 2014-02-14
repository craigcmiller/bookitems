<%@ Page Title="" Language="C#" MasterPageFile="~/ExternalSite.Master" AutoEventWireup="true" CodeBehind="UserPage.aspx.cs" Inherits="BookItems.Site.UserPageExternal" %>
<%@ Import Namespace="BookItems.Core" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	
	<br />
	<br />
	
	<div class="usercontent">
		<%=GetContent()%>
	</div>
	
</asp:Content>
