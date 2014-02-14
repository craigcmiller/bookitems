using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework.Config;

namespace BookItems.Site
{
	public class Global : System.Web.HttpApplication
	{
		void Application_Start(object sender, EventArgs e)
		{
            InitActiveRecord();
		}

        private static void InitActiveRecord()
        {
            int initAttempts = 0;
            try
            {
                // Initialise ActiveRecord
                XmlConfigurationSource source = new XmlConfigurationSource(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("BookItems.Site.ActiveRecordConfig.xml"));
                source.IsRunningInWebApp = true;
                source.SetDebugFlag(false);

                ActiveRecordStarter.Initialize(source,
                    typeof(BookItems.Core.Booking),
                    typeof(BookItems.Core.BookableItem),
                    typeof(BookItems.Core.NewsItem),
                    typeof(BookItems.Core.User),
					typeof(BookItems.Core.UserPage));

                //ActiveRecordStarter.Initialize(System.Reflection.Assembly.GetAssembly(typeof(BookItems.Core.User)), source);

				//ActiveRecordStarter.CreateSchema(typeof(BookItems.Core.UserPage));

				//Console.WriteLine("Updating schema");
				//ActiveRecordStarter.UpdateSchema();
                
                /*BookItems.Core.User user = BookItems.Core.User.Find(21);
                user.AdministeredBookableItems.Add(BookItems.Core.BookableItem.FindFirst());
                //user.CanEditCustomPages = true;
                //user.CanCreateCustomPages = true;

                user.SaveAndFlush();*/
            }
            catch (Exception ex) // TODO Log exception
            {
                EmailError("Init site " + ex.Message + " - " + ex.StackTrace);
				Console.WriteLine (ex);

                //if (initAttempts++ < 10)
                  //  InitActiveRecord();
            }
        }

		void Application_End(object sender, EventArgs e)
		{
			//  Code that runs on application shutdown

		}

        private static void EmailError(string message)
        {
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587)
                                        {
                                            EnableSsl = true,
                                            UseDefaultCredentials = false,
                                            Credentials =
                                                new NetworkCredential("shorehamsussexflyinggroup@gmail.com", "ssfggcgfg")
                                        };

            ServicePointManager.ServerCertificateValidationCallback = (s, cert, chain, sslPolicyErrors) => true;

            smtpClient.Send(
                "shorehamsussexflyinggroup@gmail.com",
                "shorehamsussexflyinggroup@gmail.com",
                "Booking system error",
                message);
        }

	    void Application_Error(object sender, EventArgs e)
		{
            Exception ex = HttpContext.Current.Server.GetLastError();
			Console.WriteLine (ex);
            EmailError(ex.Message + " - " + ex.StackTrace);

            Server.ClearError();
		}

		void Application_BeginRequest(object sender, EventArgs e)
		{
            try
            {
                HttpContext.Current.Items.Add("ar.sessionscope", new SessionScope());
            }
            catch (Exception ex)
            {
                EmailError("Application_BeginRequest - " + ex.Message + " - " + ex.StackTrace);
                throw;
            }
		}
		
		void Application_EndRequest(object sender, EventArgs e)
		{
			try
			{
				var scope = HttpContext.Current.Items["ar.sessionscope"] as SessionScope;
				if (scope != null)
					scope.Dispose();
			}
			catch (Exception ex)
			{
				HttpContext.Current.Trace.Warn("Error", "EndRequest: " + ex.Message, ex);
			}
		}

		void Session_Start(object sender, EventArgs e)
		{
			// Code that runs when a new session is started
			//HttpContext.Current.Items.Add("ar.sessionscope", new SessionScope());
		}

		void Session_End(object sender, EventArgs e)
		{
			// Code that runs when a session ends. 
			// Note: The Session_End event is raised only when the sessionstate mode
			// is set to InProc in the Web.config file. If session mode is set to StateServer 
			// or SQLServer, the event is not raised.
			/*
			try
			{
				var scope = HttpContext.Current.Items["ar.sessionscope"] as SessionScope;
				if (scope != null)
					scope.Dispose();
			}
			catch (Exception ex)
			{
				HttpContext.Current.Trace.Warn("Error", "EndRequest: " + ex.Message, ex);
			}*/
		}
	}
}
