using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using System.Xml.Serialization;

namespace BookItems.Core
{
	[ActiveRecord("NewsItems", Lazy = true)]
	public class NewsItem : EntityBase<NewsItem>
	{
		public NewsItem()
		{
		}

		[PrimaryKey]
		public virtual int Id { get; set; }

		[Property]
		public virtual DateTime CreatedDate { get; set; }

		[BelongsTo]
		public virtual BookableItem BookableItem { get; set; }

		[BelongsTo]
		public virtual User PostedByUser { get; set; }

        [Property(NotNull = true, Default = "")]
		public virtual string Title { get; set; }

        [Property(NotNull = true, Default = "", Length = 4000)]
        public virtual string BodyText { get; set; }

		[Property(Default = "0")]
		public virtual bool VisibleOnPublicPage { get; set; }
	}
}
