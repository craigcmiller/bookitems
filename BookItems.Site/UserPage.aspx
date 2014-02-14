<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserPage.aspx.cs" Inherits="BookItems.Site.UserPage" %>
<%@ Import Namespace="BookItems.Core" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	
	<br />
	<br />
	
	<div>
		<%=GetLinks()%>
	</div>
	
	<div class="usercontent">
		<%=GetContent()%>
	</div>
	
</asp:Content>


