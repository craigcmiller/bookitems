using System;
using System.Net.Mail;
using System.Net;
using System.Collections.Generic;

namespace BookItems.Core
{
	public class Emailer
	{
		private readonly SmtpClient _smtpClient;

		public Emailer()
		{
			_smtpClient = new SmtpClient ("smtp.mandrillapp.com", 587) {
				EnableSsl = true,
				UseDefaultCredentials = false,
				Credentials = new NetworkCredential("shorehamsussexflyinggroup@gmail.com", "")
			};

			ServicePointManager.ServerCertificateValidationCallback = (s, cert, chain, sslPolicyErrors) => true;
		}

		public void SendEmail(string toAddress, string title, string content)
		{
			_smtpClient.Send (
				"shorehamsussexflyinggroup@gmail.com",
				toAddress,
				title,
				content);
		}
	}
}

