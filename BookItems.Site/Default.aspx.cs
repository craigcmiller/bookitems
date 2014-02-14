using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using BookItems.Core;

namespace BookItems.Site
{
	public partial class _Default : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!SessionManager.Shared.IsLoggedIn)
				Response.Redirect("Login.aspx");
		}

		public string GetNewsHtml()
		{
			StringBuilder newsHtml = new StringBuilder();

			IList<NewsItem> newsItems = SessionManager.Shared.BookableItem.GetLatestNewsItems(3);
			for (int i = 0; i < newsItems.Count; i++)
			{
				NewsItem newsItem = newsItems[i];

				newsHtml.AppendFormat(
					"<tr><td class=\"newsitemtitle\">{0} <em>({1})</em> - <small>Posted by {2}</small></td></tr><tr><td class=\"newsitembody\">{3}</td></tr>",
					HttpUtility.HtmlEncode(newsItem.Title),
					newsItem.CreatedDate,
					HttpUtility.HtmlEncode(newsItem.PostedByUser.FirstName + " " + newsItem.PostedByUser.LastName),
					HttpUtility.HtmlEncode(newsItem.BodyText).Replace("\r\n", "<br />").Replace("\n", "<br />"));

				if (i != newsItems.Count - 1)
					newsHtml.Append("<tr class=\"newsitemdivider\"></tr>");

				newsHtml.AppendLine();
			}

			return newsHtml.ToString();
		}
	}
}
