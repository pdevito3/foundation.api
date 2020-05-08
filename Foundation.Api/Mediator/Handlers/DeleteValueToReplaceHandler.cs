namespace Foundation.Api.Mediator.Handlers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Foundation.Api.Mediator.Commands;
    using Foundation.Api.Mediator.Queries;
    using Foundation.Api.Models;
    using Foundation.Api.Services;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    public class DeleteValueToReplaceHandler : IRequestHandler<DeleteValueToReplaceCommand, IActionResult>
    {
        private readonly IValueToReplaceRepository _valueToReplaceRepository;

        public DeleteValueToReplaceHandler(IValueToReplaceRepository valueToReplaceRepository)
        {
            _valueToReplaceRepository = valueToReplaceRepository ??
                throw new ArgumentNullException(nameof(valueToReplaceRepository));
        }

        public async Task<IActionResult> Handle(DeleteValueToReplaceCommand command, CancellationToken cancellationToken)
        {
            var valueToReplaceFromRepo = _valueToReplaceRepository.GetValueToReplace(command.ValueToReplaceId);

            if (valueToReplaceFromRepo == null)
            {
                return command.Controller.NotFound();
            }

            _valueToReplaceRepository.DeleteValueToReplace(valueToReplaceFromRepo);
            _valueToReplaceRepository.Save();

            return command.Controller.NoContent();
        }
    }
}
