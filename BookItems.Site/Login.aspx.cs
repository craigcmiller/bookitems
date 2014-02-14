using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BookItems.Core;

namespace BookItems.Site
{
	public partial class Login : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

		protected void loginButton_Click(object sender, EventArgs e)
		{
			if (!SessionManager.Shared.AttemptLogin(userNameTextBox.Text, passwordTextBox.Text))
			{
				loginErrorLabel.Text = "Login failed. Please make sure that your pilot number and password are correct.";
			}
			else
			{
				Response.Redirect("Default.aspx");
			}
		}
	}
}