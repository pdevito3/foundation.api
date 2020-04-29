using Foundation.Api.Models;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foundation.Api.Mediator.Responses
{
    public class GetAllValueToReplacesQueryResponse
    {
        public IEnumerable<ValueToReplaceDto> ValueToReplaceDtoIEnumerable { get; set; }
        public KeyValuePair<string, StringValues> ResponseHeaderPagination { get; set; }

    }
}
