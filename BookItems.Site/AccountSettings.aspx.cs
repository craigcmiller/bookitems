using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BookItems.Core;

namespace BookItems.Site
{
	public partial class AccountSettings : System.Web.UI.Page
	{
		public AccountSettings()
		{
			
		}

		protected override void OnLoadComplete(EventArgs e)
		{
			base.OnLoadComplete(e);

			User user = SessionManager.Shared.User;

			emailAddressTextBox.Text = user.EmailAddress;
			firstNameTextBox.Text = user.FirstName;
			lastNameTextBox.Text = user.LastName;
			phoneNumberTextBox.Text = user.PhoneNumber;
			mobileNumberTextBox.Text = user.MobileNumber;
			emailDayBeforeBookingReminder.Checked = user.Email24HoursBeforeBooking;
		}

		protected void Page_Load(object sender, EventArgs e)
		{
		}

		protected void saveButton_OnClick(object sender, EventArgs e)
		{
			User user = SessionManager.Shared.User;

			user.EmailAddress = emailAddressTextBox.Text;
			user.FirstName = firstNameTextBox.Text;
			user.LastName = lastNameTextBox.Text;
			user.PhoneNumber = phoneNumberTextBox.Text;
			user.MobileNumber = mobileNumberTextBox.Text;
            user.Email24HoursBeforeBooking = emailDayBeforeBookingReminder.Checked;

			user.SaveAndFlush();
		}
	}
}
