<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserPage.aspx.cs" Inherits="BookItems.Site.UserPageEdit" %>
<%@ Import Namespace="BookItems.Core" %>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<script src="ckeditor/ckeditor.js"></script>
	<script>
		function formsubmit()
		{
			document.forms[0].action = 'SaveUserPage.ashx';
			document.forms[0].method = 'get';
			
			document.getElementById("html").value = CKEDITOR.instances.Editor.document.getBody().getHtml();
			
			//document.getElementById("editform").submit();
			//onclick="formsubmit();"
		}
	</script>

	<br /><br />
	
	<%=GetTitleEditor()%>
	
	<textarea class="ckeditor" name="Editor" id="Editor"><%=GetPageContent()%></textarea>
	
	
	<div>
		<%=GetPublicCheckboxValue()%>
	</div>
	<div>
		<input type="submit" value="Save" onclick="formsubmit();" />
	</div>
	
</asp:Content>


