using System;
using System.Web;
using System.Web.UI;
using BookItems.Core;

namespace BookItems.Site
{
	public partial class UserPageEdit : System.Web.UI.Page
	{
		private int _userPageId = -1;
		private BookItems.Core.UserPage _userPage;

		protected void Page_Load(object sender, EventArgs e)
		{
			string userPageIdStr = Request.QueryString ["userPageId"];
			if (userPageIdStr == null) { // Create page
				if (!SessionManager.Shared.User.CanCreateCustomPages)
					Response.Redirect ("~");
			} else { // Edit page
				if (!SessionManager.Shared.User.CanEditCustomPages)
					Response.Redirect ("~");

				_userPageId = int.Parse (userPageIdStr);
				_userPage = BookItems.Core.UserPage.Find (_userPageId);
			}
		}

		public string GetTitleEditor()
		{
			if (_userPageId == -1)
				return "<p>Page Title:<input type=\"text\" name=\"title\" /></p>";

			return string.Format("<input type=\"hidden\" name=\"userPageId\" value=\"{0}\" />", _userPageId);
		}

		public string GetPageContent()
		{
			if (_userPageId == -1) // New page
				return "";
			else // Edit
				return _userPage.ContentHtml;
		}

		public string GetPublicCheckboxValue()
		{
			if (_userPageId != -1 && _userPage.VisibleOnPublicSite)
				return "<input type=\"checkbox\" name=\"ispublic\" checked>Visible on Public Site</input>";

			return "<input type=\"checkbox\" name=\"ispublic\">Visible on Public Site</input>";
		}
	}
}

