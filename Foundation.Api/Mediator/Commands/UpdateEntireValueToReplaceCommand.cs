namespace Foundation.Api.Mediator.Commands
{
    using Foundation.Api.Models;
    using MediatR;

    public class UpdateEntireValueToReplaceCommand : IRequest<string>
    {
        public int ValueToReplaceId { get; set; }
        public ValueToReplaceForUpdateDto ValueToReplaceForUpdateDto { get; set; }

        public UpdateEntireValueToReplaceCommand(int valueToReplaceId,ValueToReplaceForUpdateDto valueToReplaceForUpdateDto)
        {
            ValueToReplaceId = valueToReplaceId;
            ValueToReplaceForUpdateDto = valueToReplaceForUpdateDto;
        }
    }
}
