using Foundation.Api.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Text;

namespace Foundation.Api.Models
{
    public class ValueToReplaceParametersDto : ValueToReplacePaginationParameters
    {
        public string Filters { get; set; }
        public string QueryString { get; set; }
        public string SortOrder { get; set; }
    }
}
