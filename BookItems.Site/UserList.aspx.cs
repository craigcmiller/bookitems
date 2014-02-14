using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Iesi.Collections.Generic;
using BookItems.Core;

namespace BookItems.Site
{
	public partial class UserList : System.Web.UI.Page
	{
        public enum UserListContent { Users = 1, Admins = 2 }

		protected void Page_Load(object sender, EventArgs e)
		{

		}
        
		public string GetUserListHtml(UserListContent userListContent)
		{
			BookableItem bookableItem = SessionManager.Shared.BookableItem;
            bookableItem.Refresh(); // Make sure we have the latest user details

			StringBuilder html = new StringBuilder();

            IList<User> userList = new List<User>();

            if ((userListContent & UserListContent.Users) == UserListContent.Users)
                userList.AddCollection(bookableItem.Users);
            if ((userListContent & UserListContent.Admins) == UserListContent.Admins)
                userList.AddCollection(bookableItem.Administrators);

			foreach (User user in userList)
			{
                string elevatedPrivilegesHtml;
                if (HasElevatedPrivileges)
                {
                    elevatedPrivilegesHtml =
                        string.Format("<td><a href=\"UpdateUser.ashx?\">Change Password</a></td><td><a href=\"\">Reset</a></td><td><a href=\"\">Delete</a></td>");
                }
                else elevatedPrivilegesHtml = "";

                html.AppendFormat(
                    "<tr><td>{0}</td><td>{1}</td><td>{2}</td><td><a href=\"mailto:{3}\">{3}</a></td><td><a href=\"tel://{4}\">{4}</a></td><td><a href=\"tel://{5}\">{5}</a></td>{6}</tr>",
                    HttpUtility.HtmlEncode(user.Username),
                    HttpUtility.HtmlEncode(user.FirstName),
                    HttpUtility.HtmlEncode(user.LastName),
                    HttpUtility.HtmlEncode(user.EmailAddress),
                    HttpUtility.HtmlEncode(user.PhoneNumber),
                    HttpUtility.HtmlEncode(user.MobileNumber),
                    elevatedPrivilegesHtml
                    );
			}

			return html.ToString();
		}

        /// <summary>
        /// Gets if the current user is allowed to delete accounts, reset passwords, etc
        /// </summary>
        public bool HasElevatedPrivileges
        {
            get
            {
                return SessionManager.Shared.BookableItem.HasAdminUser(SessionManager.Shared.User) && SessionManager.Shared.BookableItem.HasUser(SessionManager.Shared.User);
            }
        }
	}
}
