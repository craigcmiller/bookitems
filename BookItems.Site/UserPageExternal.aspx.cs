using System;
using System.Web;
using System.Web.UI;

namespace BookItems.Site
{
	public partial class UserPageExternal : System.Web.UI.Page
	{
		private int _userPageId;

		protected void Page_Load(object sender, EventArgs e)
		{
			_userPageId = int.Parse (Request.QueryString ["userPageId"]);
		}

		public string GetContent()
		{
			return BookItems.Core.UserPage.Find (_userPageId).ContentHtml;
		}
	}
}

