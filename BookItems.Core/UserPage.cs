using System;
using Castle.ActiveRecord;
using System.Collections.Generic;
using System.Linq;

namespace BookItems.Core
{
	[ActiveRecord("UserPages", Lazy = true)]
	public class UserPage : EntityBase<UserPage>
	{
		public static UserPage Create(BookableItem bookableItem, string title, string html)
		{
			UserPage userPage = new UserPage {
				BookableItem = bookableItem,
				Title = title,
				ContentHtml = html,
				Created = DateTime.Now,
				LastUpdated = DateTime.Now
			};

			return userPage;
		}

		public static IList<UserPage> GetPublicallyVisibleUserPages()
		{
			return UserPage.FindAll ().Where (up => up.VisibleOnPublicSite).ToList () ?? new List<UserPage> ();
		}

		public UserPage ()
		{
		}

		[PrimaryKey]
		public virtual int Id { get; set; }

		[BelongsTo]
		public virtual BookableItem BookableItem { get; set; }

		[Property(NotNull = true, Length = 128)]
		public virtual string Title { get; set; }

		[Property(NotNull = true, Length = 4000, Default = "")]
		public virtual string ContentHtml { get; set; }

		[Property(NotNull = true)]
		public virtual DateTime Created { get; set; }

		[Property(NotNull = true)]
		public virtual DateTime LastUpdated { get; set; }

		[Property(NotNull = true, Default = "0")]
		public virtual bool VisibleOnPublicSite { get; set; }
	}
}

