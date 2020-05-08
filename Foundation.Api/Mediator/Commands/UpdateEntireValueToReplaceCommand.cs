namespace Foundation.Api.Mediator.Commands
{
    using Foundation.Api.Models;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    public class UpdateEntireValueToReplaceCommand : IRequest<IActionResult>
    {
        public int ValueToReplaceId { get; set; }
        public ValueToReplaceForUpdateDto ValueToReplaceForUpdateDto { get; set; }
        public Controller Controller { get; set; }

        public UpdateEntireValueToReplaceCommand(int valueToReplaceId,ValueToReplaceForUpdateDto valueToReplaceForUpdateDto, Controller controller)
        {
            ValueToReplaceId = valueToReplaceId;
            ValueToReplaceForUpdateDto = valueToReplaceForUpdateDto;
            Controller = controller;
        }
    }
}
