namespace Foundation.Api.Mediator.Queries
{
    using Foundation.Api.Mediator.Responses;
    using Foundation.Api.Models;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    public class GetAllValueToReplacesQuery : IRequest<IActionResult>
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
