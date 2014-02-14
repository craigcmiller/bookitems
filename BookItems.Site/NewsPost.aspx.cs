using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BookItems.Core;

namespace BookItems.Site
{
	public partial class NewsPost : System.Web.UI.Page
	{
		private const string DELETE_BY_ID = "deleteId";

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(Request.QueryString[DELETE_BY_ID]))
			{
				BookableItem bookableItem = SessionManager.Shared.BookableItem;
				User user = SessionManager.Shared.User;

				int id = int.Parse(Request.QueryString[DELETE_BY_ID]);

				NewsItem newsItemToDelete = NewsItem.Find(id);
				if (newsItemToDelete.PostedByUser.Id == user.Id
					|| (bookableItem.HasAdminUser(user) && bookableItem.HasUser(user)))
					newsItemToDelete.DeleteAndFlush();
			}
		}

		protected void postNewsButton_Click(object sender, EventArgs e)
		{
			SessionManager.Shared.BookableItem.CreateNewsItem(
				titleTextBox.Text, messageBodyTextBox.Text, SessionManager.Shared.User);

			Response.Redirect("~/Default.aspx");
		}
	}
}
