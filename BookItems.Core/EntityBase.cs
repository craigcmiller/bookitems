using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;

namespace BookItems.Core
{
	
	public class EntityBase<T> : ActiveRecordBase<T>
	{
		/// <summary>
		/// Finds the first value by property. If nothing is found returns null
		/// </summary>
		/// <param name="column"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		protected static T FindByProperty(string column, object value)
		{
			T[] users = FindAllByProperty(column, value);
			if (users.Length > 0)
				return users[0];

			return default(T);
		}
	}
}
