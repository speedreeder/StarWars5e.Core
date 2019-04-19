using System.Collections.Generic;
using System.Linq;

namespace StarWars5e.Models.Search
{
    public class PagedSearchResult<T>
    {
        public PagedSearchResult(ICollection<T> data, int pageSize, int currentPage)
        {
            Total = data.Count;
            PageSize = pageSize;
            CurrentPage = currentPage;
            Data = data.Skip((currentPage - 1) * pageSize).Take(PageSize).ToList();

        }
        public IList<T> Data { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int Total { get; }
    }
}
