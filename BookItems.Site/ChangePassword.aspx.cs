using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BookItems.Core;

namespace BookItems.Site
{
	public partial class ChangePassword : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

		public void changePasswordButton_OnClick(object sender, EventArgs args)
		{
            if (SessionManager.Shared.User.Password == currentPasswordTextBox.Text)
            {
                if (newPasswordTextBox.Text != confirmPasswordTextBox.Text)
                {
                    errorLabel.Text = "New password and confirmation must match";
                }
                else if (newPasswordTextBox.Text.Length < 6)
                {
                    errorLabel.Text = "Minimum password length of 6 characters";
                }
                else
                {
                    try
                    {
                        SessionManager.Shared.User.ChangePassword(currentPasswordTextBox.Text, newPasswordTextBox.Text);

                        Response.Redirect("~/Login.aspx");
                    }
                    catch (ArgumentException ae)
                    {
                        errorLabel.Text = ae.Message;
                    }
                }
            }
            else
                errorLabel.Text = "Your current password does not match";
		}
	}
}
