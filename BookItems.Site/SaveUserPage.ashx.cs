using System;
using System.Web;
using System.Web.UI;
using BookItems.Core;

namespace BookItems.Site
{
	public class SaveUserPage : System.Web.IHttpHandler
	{
		
		public bool IsReusable {
			get {
				return false;
			}
		}

		public void ProcessRequest (HttpContext context)
		{
			string html = context.Request.QueryString ["Editor"];
			string userPageIdStr = context.Request.QueryString ["userPageId"];
			bool isPublic = context.Request.QueryString ["ispublic"] == "on";

			if (userPageIdStr == null) { // Create
				BookItems.Core.UserPage newPage = BookItems.Core.UserPage.Create (SessionManager.Shared.BookableItem, context.Request.QueryString ["title"], html);
				newPage.VisibleOnPublicSite = isPublic;
				newPage.SaveAndFlush ();

				context.Response.Redirect (string.Format ("UserPage.aspx?userPageId={0}", newPage.Id));
			} else { // Edit or delete
				int userPageId = int.Parse (userPageIdStr);
				BookItems.Core.UserPage page = BookItems.Core.UserPage.Find (userPageId);

				if (context.Request.QueryString ["delete"] == null) { // Edit
					page.VisibleOnPublicSite = isPublic;
					page.ContentHtml = html;

					page.SaveAndFlush ();

					context.Response.Redirect (string.Format ("UserPage.aspx?userPageId={0}", userPageId));
				} else { // Delete
					page.DeleteAndFlush ();

					context.Response.Redirect ("Default.aspx");
				}
			}
		}
	}
}

