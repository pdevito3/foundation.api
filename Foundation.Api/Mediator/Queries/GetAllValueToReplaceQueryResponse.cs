using Foundation.Api.Models;
using Foundation.Api.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foundation.Api.Mediator.Queries
{
    public class GetAllValueToReplaceQueryResponse
    {
        public IEnumerable<ValueToReplaceDto> ValueToReplaceDtoList { get; set; }
        public PaginationHeader PaginationMetadata { get; set; }
    }
}
