<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewsListingUserControl.ascx.cs" Inherits="BookItems.Site.NewsListingUserControl" %>
<%@ Import Namespace="BookItems.Core" %>

<div id="newsdisplayarea">
	<table width="100%">
		<tr>
			<th align="center">News</th>
		</tr>
		<tr class="newsitemdivider"></tr>
		
		<%=GetNewsHtml(!SessionManager.Shared.IsLoggedIn) %>
	</table>
</div>
