using System;
using System.Collections.Generic;
using System.Text;

namespace StarWars5e.Models.Search
{
    public class SearchBase
    {
        public int? Total { get; set; }
        public int? Page { get; set; }
        public int? RecordsPerPage { get; set; }
    }
}
