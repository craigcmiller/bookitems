<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserList.aspx.cs" Inherits="BookItems.Site.UserList" %>
<%@ Import Namespace="BookItems.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

	<br /><br />
	
	<div id="userlistarea" align="center">
		<table>
			<tr>
				<th>Pilot</th>
				<th>First Name</th>
				<th>Last Name</th>
				<th>Email Address</th>
				<th>Phone Number</th>
				<th>Mobile Number</th>
			</tr>
			
			<%=GetUserListHtml(UserListContent.Users) %>
		</table>
		
		<%if (HasElevatedPrivileges)
        {%>
            <h2>Administrators</h2>
            
            <table>
                <tr>
				    <th>Pilot</th>
				    <th>First Name</th>
				    <th>Last Name</th>
				    <th>Email Address</th>
				    <th>Phone Number</th>
				    <th>Mobile Number</th>
			    </tr>
    			
		        <%=GetUserListHtml(UserListContent.Admins) %>
		    </table>
		<%} %>
	</div>

</asp:Content>
