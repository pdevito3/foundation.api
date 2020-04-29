namespace Foundation.Api.Mediator.Queries
{
    using Foundation.Api.Mediator.Responses;
    using Foundation.Api.Models;
    using MediatR;

    public class GetAllValueToReplacesQuery : IRequest<GetAllValueToReplacesQueryResponse>
    {
        public ValueToReplaceParametersDto ValueToReplaceParametersDto { get; }

        public GetAllValueToReplacesQuery(ValueToReplaceParametersDto valueToReplaceParametersDto)
        {
            ValueToReplaceParametersDto = valueToReplaceParametersDto;
        }
    }
}
