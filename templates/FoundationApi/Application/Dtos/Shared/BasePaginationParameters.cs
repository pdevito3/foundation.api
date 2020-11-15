namespace Application.Dtos.Shared
{
    public abstract class BasePaginationParameters
    {
        public virtual int MaxPageSize { get; } = 20;
        public virtual int PageNumber { get; set; } = 1;

        public virtual int DefaultPageSize { get; set; } = 10;

        public int PageSize
        {
            get
            {
                return DefaultPageSize;
            }
            set
            {
                DefaultPageSize = value > MaxPageSize ? MaxPageSize : value;
            }
        }
    }
}
