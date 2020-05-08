namespace Foundation.Api.Mediator.Handlers
{
    using AutoMapper;
    using Foundation.Api.Mediator.Commands;
    using Foundation.Api.Models;
    using Foundation.Api.Services;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class UpdatePartialValueToReplaceHandler : IRequestHandler<UpdatePartialValueToReplaceCommand, IActionResult>
    {
        private readonly IValueToReplaceRepository _valueToReplaceRepository;
        private readonly IMapper _mapper;

        public UpdatePartialValueToReplaceHandler(IValueToReplaceRepository valueToReplaceRepository
            , IMapper mapper)
        {
            _valueToReplaceRepository = valueToReplaceRepository ??
                throw new ArgumentNullException(nameof(valueToReplaceRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IActionResult> Handle(UpdatePartialValueToReplaceCommand updatePartialValueToReplaceCommand, CancellationToken cancellationToken)
        {
            if (updatePartialValueToReplaceCommand.PatchDoc == null)
            {
                return updatePartialValueToReplaceCommand.Controller.BadRequest();
            }

            var existingValueToReplace = _valueToReplaceRepository.GetValueToReplace(updatePartialValueToReplaceCommand.ValueToReplaceId);

            if (existingValueToReplace == null)
            {
                return updatePartialValueToReplaceCommand.Controller.NotFound();
            }

            var valueToReplaceToPatch = _mapper.Map<ValueToReplaceForUpdateDto>(existingValueToReplace); // map the valueToReplace we got from the database to an updatable valueToReplace model
            updatePartialValueToReplaceCommand.PatchDoc.ApplyTo(valueToReplaceToPatch, updatePartialValueToReplaceCommand.Controller.ModelState); // apply patchdoc updates to the updatable valueToReplace

            if (!updatePartialValueToReplaceCommand.Controller.TryValidateModel(valueToReplaceToPatch))
            {
                return updatePartialValueToReplaceCommand.Controller.ValidationProblem(updatePartialValueToReplaceCommand.Controller.ModelState);
            }

            _mapper.Map(valueToReplaceToPatch, existingValueToReplace); // apply updates from the updatable valueToReplace to the db entity so we can apply the updates to the database
            _valueToReplaceRepository.UpdateValueToReplace(existingValueToReplace); // apply business updates to data if needed

            _valueToReplaceRepository.Save(); // save changes in the database

            return updatePartialValueToReplaceCommand.Controller.NoContent();
        }
    }
}
