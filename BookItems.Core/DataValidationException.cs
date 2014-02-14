using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookItems.Core
{
	public class DataValidationException : Exception
	{
		public DataValidationException(string message)
			: base(message)
		{
		}
	}
}
