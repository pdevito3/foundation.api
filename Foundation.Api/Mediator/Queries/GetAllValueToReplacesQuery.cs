namespace Foundation.Api.Mediator.Queries
{
    using Foundation.Api.Models;
    using Foundation.Api.Models.Pagination;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    public class GetAllValueToReplacesQuery : IRequest<GetAllValueToReplaceQueryResponse>
    {
        public ValueToReplaceParametersDto ValueToReplaceParametersDto { get; }
        public Controller Controller { get; }

        public GetAllValueToReplacesQuery(ValueToReplaceParametersDto valueToReplaceParametersDto, Controller controller)
        {
            ValueToReplaceParametersDto = valueToReplaceParametersDto;
            Controller = controller;
        }
    }
}
