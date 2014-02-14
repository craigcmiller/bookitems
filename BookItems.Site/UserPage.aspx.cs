using System;
using System.Web;
using System.Web.UI;
using BookItems.Core;
using System.Text;

namespace BookItems.Site
{
	public partial class UserPage : System.Web.UI.Page
	{
		private int _userPageId;

		protected void Page_Load(object sender, EventArgs e)
		{
			_userPageId = int.Parse (Request.QueryString ["userPageId"]);
		}

		public string GetLinks()
		{
			StringBuilder linksText = new StringBuilder ();

			if (SessionManager.Shared.User.CanEditCustomPages)
				linksText.AppendFormat ("<a href=\"UserPageEdit.aspx?userPageId={0}\">Edit</a>", _userPageId);
			if (SessionManager.Shared.User.CanCreateCustomPages)
				linksText.AppendFormat (" | <a href=\"SaveUserPage.ashx?userPageId={0}&delete=true\">Delete</a>", _userPageId);

			return linksText.ToString ();
		}

		public string GetContent()
		{
			return BookItems.Core.UserPage.Find (_userPageId).ContentHtml;
		}
	}
}
