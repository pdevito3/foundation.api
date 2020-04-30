namespace Foundation.Api.Mediator.Responses
{
    using Foundation.Api.Models;
    using Microsoft.Extensions.Primitives;
    using System.Collections.Generic;

    public class GetAllValueToReplacesQueryResponse
    {
        public IEnumerable<ValueToReplaceDto> ValueToReplaceDtoIEnumerable { get; set; }
        public KeyValuePair<string, StringValues> ResponseHeaderPagination { get; set; }

    }
}
