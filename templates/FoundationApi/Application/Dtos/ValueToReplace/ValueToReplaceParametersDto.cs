namespace Application.Dtos.ValueToReplace
{
    using Application.Dtos.Shared;

    public class ValueToReplaceParametersDto : BasePaginationParameters
    {
        public string Filters { get; set; }
        public string QueryString { get; set; }
        public string SortOrder { get; set; }
    }
}
