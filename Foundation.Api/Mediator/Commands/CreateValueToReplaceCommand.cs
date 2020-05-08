namespace Foundation.Api.Mediator.Commands
{
    using Foundation.Api.Models;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    public class CreateValueToReplaceCommand : IRequest<ActionResult<ValueToReplaceDto>>
    {
        public ValueToReplaceForCreationDto ValueToReplaceForCreationDto { get; set; }
        public Controller Controller { get; set; }

        public CreateValueToReplaceCommand(ValueToReplaceForCreationDto valueToReplaceForCreationDto, Controller controller)
        {
            ValueToReplaceForCreationDto = valueToReplaceForCreationDto;
            Controller = controller;
        }
    }
}
