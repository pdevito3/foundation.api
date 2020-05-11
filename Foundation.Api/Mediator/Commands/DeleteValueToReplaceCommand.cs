namespace Foundation.Api.Mediator.Commands
{
    using MediatR;

    public class DeleteValueToReplaceCommand : IRequest<bool>
    {
        public int ValueToReplaceId { get; }

        public DeleteValueToReplaceCommand(int valueToReplaceId)
        {
            ValueToReplaceId = valueToReplaceId;
        }
    }
}
