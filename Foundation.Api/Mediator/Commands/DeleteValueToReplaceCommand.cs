namespace Foundation.Api.Mediator.Commands
{
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    public class DeleteValueToReplaceCommand : IRequest<IActionResult>
    {
        public int ValueToReplaceId { get; }
        public Controller Controller { get; set; }

        public DeleteValueToReplaceCommand(int valueToReplaceId, Controller controller)
        {
            ValueToReplaceId = valueToReplaceId;
            Controller = controller;
        }
    }
}
