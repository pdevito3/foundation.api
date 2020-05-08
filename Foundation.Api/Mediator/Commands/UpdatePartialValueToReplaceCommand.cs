namespace Foundation.Api.Mediator.Commands
{
    using Foundation.Api.Models;
    using MediatR;
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.AspNetCore.Mvc;

    public class UpdatePartialValueToReplaceCommand : IRequest<IActionResult>
    {
        public int ValueToReplaceId { get; set; }
        public JsonPatchDocument<ValueToReplaceForUpdateDto> PatchDoc { get; set; }
        public Controller Controller { get; set; }

        public UpdatePartialValueToReplaceCommand(int valueToReplaceId, JsonPatchDocument<ValueToReplaceForUpdateDto> patchDoc,
            Controller controller)
        {
            ValueToReplaceId = valueToReplaceId;
            PatchDoc = patchDoc;
            Controller = controller;
        }
    }
}
