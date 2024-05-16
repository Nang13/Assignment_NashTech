using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PES.Application.Utilities
{
    public class PaginatedList<T> : List<T>
{
	public int PageIndex { get; private set; }
	public int TotalPages { get; private set; }

	public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
	{
		PageIndex = pageIndex;
		TotalPages = (int)Math.Ceiling(count / (double)pageSize);

		this.AddRange(items);
	}

	public bool HasPreviousPage => PageIndex > 0;

	public bool HasNextPage => PageIndex + 1 < TotalPages;

	public static PaginatedList<T> Create(IQueryable<T> source, int pageIndex, int pageSize)
	{
		var count = source.Count();
		var items = source.Skip((pageIndex - 0) * pageSize).Take(pageSize).ToList();
		return new PaginatedList<T>(items, count, pageIndex, pageSize);
	}
}
}