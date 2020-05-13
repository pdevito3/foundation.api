namespace Foundation.Api.Models.Pagination
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class PaginationHeader
    {
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public bool HasPrevious { get; set; }
        public bool HasNext { get; set; }
        public string PreviousPageLink { get; set; }
        public string NextPageLink { get; set; }
    }
}
