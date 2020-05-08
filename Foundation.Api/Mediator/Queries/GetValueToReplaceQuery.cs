namespace Foundation.Api.Mediator.Queries
{
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    public class GetValueToReplaceQuery : IRequest<IActionResult>
    {
        public int ValueToReplaceId { get; }
        public Controller Controller { get; set; }

        public GetValueToReplaceQuery(int valueToReplaceId, Controller controller)
        {
            ValueToReplaceId = valueToReplaceId;
            Controller = controller;
        }
    }
}
