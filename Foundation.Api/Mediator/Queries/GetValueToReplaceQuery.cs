namespace Foundation.Api.Mediator.Queries
{
    using MediatR;

    public class GetValueToReplaceQuery : IRequest<ValueToReplaceDto>
    {
        public int ValueToReplaceId { get; }

        public GetValueToReplaceQuery(int valueToReplaceId)
        {
            ValueToReplaceId = valueToReplaceId;
        }
    }
}
