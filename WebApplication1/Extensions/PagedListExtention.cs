using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace System.Linq.Extention
{
	public static class PagedListExtention
	{
		public static PageList<T> SkipAndTakeList<T>(this IQueryable<T> datalist, int nowPage, int pageSize)
		{
			var itemList = new PageList<T>(nowPage, pageSize, datalist);

			return itemList;
		}

		public static PageList<TResult> CompileSelect<T, TResult>(this I_PageList<T> source, Expression<Func<T, TResult>> exprs)
		{
			var datalist = source.Select(exprs.Compile()).AsQueryable();

			PageList<TResult> newSource = new PageList<TResult>(source.Pageinfo, datalist);

			return newSource;
		}
	}
}
