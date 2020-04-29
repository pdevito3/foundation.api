namespace Foundation.Api.Mediator.Queries
{
    using Foundation.Api.Mediator.Responses;
    using Foundation.Api.Models;
    using MediatR;

    public class GetAllValueToReplacesQuery : IRequest<GetAllValueToReplacesQueryResponse>
    {
        public ValueToReplaceParametersDto ValueToReplaceParametersDto { get; }
        public string PreviousPageLink { get; }
        public string NextPageLink { get; }

        public GetAllValueToReplacesQuery(ValueToReplaceParametersDto valueToReplaceParametersDto, string previousPageLink, string nextPageLink)
        {
            ValueToReplaceParametersDto = valueToReplaceParametersDto;
            PreviousPageLink = previousPageLink;
            NextPageLink = nextPageLink;
        }
    }
}
