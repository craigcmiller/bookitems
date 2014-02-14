using System;
using System.Net.Mail;

namespace BookItems.Core
{
	public static class MailSender
	{
		public static void SendMessage(string fromAddress, string toAddress, string subject, string body)
		{
			SmtpClient smtpClient = new SmtpClient ("http://mail.shorehamsussexflyinggroup.com");
			smtpClient.SendAsync ("", toAddress, subject, body, null);
		}
	}
}

