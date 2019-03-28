using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Generic
{
	public class PageInfo
	{
		public int TotalCount { get; set; }

		public int NowPage { get; set; }

		public int MaxPage { get; set; }

		public int PageSize { get; set; }

		public int SkipCount { get; set; }

		public bool IsOverange { get; set; }

		public PageInfo()
		{

		}

		public PageInfo(int totalCount, int nowPage, int pageSize)
		{
			this.TotalCount = totalCount;


			this.PageSize = pageSize < 1 ? 1 : pageSize;

			SetMaxPageAndNowPage(nowPage, this.TotalCount / this.PageSize);

			this.SkipCount = (this.NowPage - 1) * this.PageSize;
		}

		public void SetMaxPageAndNowPage(int nowPage, int totalpage)
		{
			this.MaxPage = this.TotalCount % this.PageSize == 0 ? totalpage : totalpage + 1;
			if (this.MaxPage >= nowPage)
			{
				this.NowPage = nowPage < 1 ? 1 : nowPage;

			}
			else
			{
				IsOverange = true;
				if (this.MaxPage <= 0)
				{
					this.MaxPage = 1;

				}
				this.NowPage = MaxPage;
			}
		}
	}

	public interface I_PageList<T> : IQueryable<T>
	{
		PageInfo Pageinfo { get; set; }

		IQueryable<T> DataList { get; set; }

		T t { get; set; }
	}

	public class PageList<T> : I_PageList<T>
	{
		public PageInfo Pageinfo { get; set; }

		private IQueryable<T> _dataList { get; set; }

		public IQueryable<T> DataList
		{
			get
			{

				return _dataList;
			}
			set
			{
				_dataList = value;

			}
		}

		public T t { get; set; }

		public PageList(int nowPage, int pageSize, IQueryable<T> DataList)
		{
			bool hasdata = true;

			try
			{
				Pageinfo = new PageInfo(DataList.Count(), nowPage, pageSize);

				hasdata = Pageinfo.TotalCount > 0;
			}
			catch
			{

				hasdata = false;
			}

			if (hasdata)
			{
				this.DataList = DataList.Skip(Pageinfo.SkipCount).Take(Pageinfo.PageSize);

			}
			else
			{
				this.DataList = Enumerable.Empty<T>().AsQueryable();
			}
		}

		public PageList(PageInfo pageinfo, IQueryable<T> DataList)
		{
			this.Pageinfo = pageinfo;
			this.DataList = DataList;
		}

		public PageList(PageInfo pageinfo, List<T> DataList)
		{
			this.Pageinfo = pageinfo;
			this.DataList = DataList.AsQueryable();
		}

		public Expression Expression
		{
			get
			{
				return DataList.Expression;
			}
		}

		public Type ElementType
		{
			get
			{
				return DataList.ElementType;
			}
		}

		public IQueryProvider Provider
		{
			get
			{
				return DataList.Provider;
			}
		}

		public IEnumerator<T> GetEnumerator()
		{
			return DataList.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return DataList.GetEnumerator();
		}
	}
}
