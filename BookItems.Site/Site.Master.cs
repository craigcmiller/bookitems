using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BookItems.Core;
using System.Text;

namespace BookItems.Site
{
	public partial class SiteMaster : System.Web.UI.MasterPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
            if (SessionManager.Shared.IsLoggedIn)
            {
                //HeadLoginView.Controls.Clear();

                //HeadLoginView.Controls.Add(new Label { Text = SessionManager.Shared.User.Username });
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
		}

		public string GetUserPages()
		{
			StringBuilder pageList = new StringBuilder ();

			foreach (BookItems.Core.UserPage userPage in BookItems.Core.UserPage.FindAll ()) {
				pageList.AppendFormat ("<a href=\"UserPage.aspx?userPageId={0}\">{1}</a>", userPage.Id, userPage.Title);
			}
			if (SessionManager.Shared.User.CanCreateCustomPages)
				pageList.Append ("<a href=\"UserPageEdit.aspx\">+</a>");

			return pageList.ToString ();
		}
	}
}
