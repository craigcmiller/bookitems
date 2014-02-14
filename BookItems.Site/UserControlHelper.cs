using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.IO;

namespace BookItems.Site
{
	public static class UserControlHelper
	{
		public delegate void InitializeControlDelegate<T>(T controlToUse);

		public static string RenderUserControlToString<T>(string controlPath, InitializeControlDelegate<T> init) where T : UserControl
		{
			Page pageHolder = new Page();
			T control = (T)pageHolder.LoadControl(controlPath);
			if (init != null)
				init(control);
			pageHolder.Controls.Add(control);
			StringWriter result = new StringWriter();
			System.Web.HttpContext.Current.Server.Execute(pageHolder, result, false);
			return result.ToString();
		}
	}
}