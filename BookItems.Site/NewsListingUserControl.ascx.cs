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
	public partial class NewsListingUserControl : System.Web.UI.UserControl
	{
		private int _newsItemsToDisplay = 1;
        private bool _showDeleteButton = false;

		protected void Page_Load(object sender, EventArgs e)
		{

		}

		public string GetNewsHtml(bool visibleToPublicOnly)
		{
			bool userIsAdmin = !visibleToPublicOnly && SessionManager.Shared.BookableItem.HasAdminUser(SessionManager.Shared.User)
				&& SessionManager.Shared.BookableItem.HasUser(SessionManager.Shared.User);

			StringBuilder newsHtml = new StringBuilder();

			BookableItem bookableItem = SessionManager.Shared.BookableItem;

			IList<NewsItem> newsItems = visibleToPublicOnly ? bookableItem.GetLatestPublicNewsItems(4) : bookableItem.GetLatestNewsItems(_newsItemsToDisplay);
			for (int i = 0; i < newsItems.Count; i++)
			{
				NewsItem newsItem = newsItems[i];

				bool wasCreatedByUser = SessionManager.Shared.User.Id == newsItem.PostedByUser.Id;

                newsHtml.AppendFormat(
                    "<tr><td class=\"newsitemtitle\">{0} <em>({1})</em> - <small>Posted by {2}</small>{4}</td></tr><tr><td class=\"newsitembody\">{3}</td></tr>",
                    HttpUtility.HtmlEncode(newsItem.Title),
                    newsItem.CreatedDate,
                    HttpUtility.HtmlEncode(newsItem.PostedByUser.FirstName + " " + newsItem.PostedByUser.LastName),
                    HttpUtility.HtmlEncode(newsItem.BodyText).Replace("\r\n", "<br />").Replace("\n", "<br />"),
                    ((userIsAdmin || wasCreatedByUser) && _showDeleteButton) ? "<a class=\"deletenewsitemlink\" href=\"NewsPost.aspx?deleteId=" + newsItem.Id + "\">X</a>" : "");

				if (i != newsItems.Count - 1)
					newsHtml.Append("<tr class=\"newsitemdivider\"></tr>");

				newsHtml.AppendLine();
			}

			return newsHtml.ToString();
		}

		/// <summary>
		/// Gets or sets the maximum number of news items to display
		/// </summary>
		public int NewsItemsToDisplay
		{
			get { return _newsItemsToDisplay; }
			set { _newsItemsToDisplay = value; }
		}

        public bool ShowDeleteButton
        {
            get { return _showDeleteButton; }
            set { _showDeleteButton = value; }
        }
	}
}