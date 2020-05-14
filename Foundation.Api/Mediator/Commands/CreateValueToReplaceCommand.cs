namespace Foundation.Api.Mediator.Commands
{
    using Foundation.Api.Models;
    using MediatR;

    public class CreateValueToReplaceCommand : IRequest<ValueToReplaceDto>
    {
        public ValueToReplaceForCreationDto ValueToReplaceForCreationDto { get; }

        public CreateValueToReplaceCommand(ValueToReplaceForCreationDto valueToReplaceForCreationDto)
        {
            ValueToReplaceForCreationDto = valueToReplaceForCreationDto;
        }
    }
}
