namespace Foundation.Api.Mediator.Commands
{
    using Foundation.Api.Models;
    using MediatR;

    public class CreateValueToReplaceCommand : ValueToReplaceForCreationDto, IRequest<ValueToReplaceDto>
    {
    }
}
