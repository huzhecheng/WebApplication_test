using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.ViewModels
{
	public class Search
	{
		public string ColumnName { get; set; }

		public object SearchValue { get; set; }

		public string SearchStatus { get; set; }

		public string ExternalCondition { get; set; }
	}
}