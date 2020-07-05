namespace Foundation.Api.Models.ValueToReplace
{
    using Foundation.Api.Models.Pagination;

    public class ValueToReplaceParametersDto : ValueToReplacePaginationParameters
    {
        public string Filters { get; set; }
        public string QueryString { get; set; }
        public string SortOrder { get; set; }
    }
}
