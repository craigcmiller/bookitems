using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using BookItems.Core;
using System.Text;

namespace BookItems.Site
{
    public partial class ExternalSite : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

		public string GetUserPages()
		{
			StringBuilder pageList = new StringBuilder ("<a href=\"Login.aspx\">Login</a>");

			foreach (BookItems.Core.UserPage userPage in BookItems.Core.UserPage.GetPublicallyVisibleUserPages ()) {
				pageList.AppendFormat ("<a href=\"UserPageExternal.aspx?userPageId={0}\">{1}</a>", userPage.Id, userPage.Title);
			}

			return pageList.ToString ();
		}
    }
}
