using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Models.ViewModels;

namespace System.Linq.Extention
{
	public static class IQueryableExtensions
	{
		public static IQueryable<T> SearchQuery<T>(this IQueryable<T> list, string searchcondition)
		{
			if (string.IsNullOrEmpty(searchcondition))
			{
				return list;
			}

			var searchModel = JsonConvert.DeserializeObject<List<Search>>(searchcondition);

			var group = searchModel.GroupBy(x => x.ColumnName);

			foreach (var gp in group)
			{
				var strQuery = "";
				var valueList = new List<object>();
				int i = 0;

				foreach (var item in gp)
				{
					var value = item.SearchValue.ToString();

					if (!string.IsNullOrEmpty(value))
					{
						string query = "";
						object SearchValue = item.SearchValue;

						switch (item.SearchStatus)
						{
							case "DateMax":
								query = string.Format("DbFunctions.TruncateTime({0}) >= DbFunctions.TruncateTime(@{1})", item.ColumnName, i);
								SearchValue = Convert.ToDateTime(item.SearchValue);
								break;
							case "DateMin":
								query = string.Format("DbFunctions.TruncateTime({0}) <= DbFunctions.TruncateTime(@{1})", item.ColumnName, i);
								SearchValue = Convert.ToDateTime(item.SearchValue);
								break;
							case "DateEqual":
								query = string.Format("DbFunctions.TruncateTime({0}) == DbFunctions.TruncateTime(@{1})", item.ColumnName, i);
								SearchValue = Convert.ToDateTime(item.SearchValue);
								break;
							case "Like":
								query = string.Format("{0}.Contains(@{1})", item.ColumnName, i);
								break;
							case "StartWith":
								query = string.Format("{0}.StartsWith(@{1})", item.ColumnName, i);
								break;
							case "EndWith":
								query = string.Format("{0}.EndsWith(@{1})", item.ColumnName, i);
								break;
							case "Equal":
								query = string.Format("{0} == (@{1})", item.ColumnName, i);
								break;
							case "Max":
								query = string.Format("{0} > (@{1})", item.ColumnName, i);
								break;
							case "Min":
								query = string.Format("{0} < (@{1})", item.ColumnName, i);
								break;
							case "MaxEqual":
								query = string.Format("{0} >= (@{1})", item.ColumnName, i);
								break;
							case "MinEqual":
								query = string.Format("{0} <= (@{1})", item.ColumnName, i);
								break;
						}

						if (gp.Count() > 1 && i != gp.Count() - 1)
						{
							if (item.ExternalCondition == null)
							{
								item.ExternalCondition = "and";
							}

							query = query + item.ExternalCondition + " ";
						}

						strQuery += query;
						valueList.Add(SearchValue);
						i++;
					}
				}

				if (!string.IsNullOrEmpty(strQuery))
				{
					list = list.Where(strQuery, valueList.ToArray());
				}
			}
			return list;
		}
	}
}
