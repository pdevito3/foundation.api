namespace Foundation.Api.Mediator.Handlers
{
    using AutoMapper;
    using Foundation.Api.Data.Entities;
    using Foundation.Api.Mediator.Commands;
    using Foundation.Api.Models;
    using Foundation.Api.Services;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class CreateValueToReplaceHandler : IRequestHandler<CreateValueToReplaceCommand, ActionResult<ValueToReplaceDto>>
    {
        private readonly IValueToReplaceRepository _valueToReplaceRepository;
        private readonly IMapper _mapper;

        public CreateValueToReplaceHandler(IValueToReplaceRepository valueToReplaceRepository
            , IMapper mapper)
        {
            _valueToReplaceRepository = valueToReplaceRepository ??
                throw new ArgumentNullException(nameof(valueToReplaceRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ActionResult<ValueToReplaceDto>> Handle(CreateValueToReplaceCommand createValueToReplaceCommand, CancellationToken cancellationToken)
        {
            var valueToReplace = _mapper.Map<ValueToReplace>(createValueToReplaceCommand.ValueToReplaceForCreationDto);
            _valueToReplaceRepository.AddValueToReplace(valueToReplace);
            _valueToReplaceRepository.Save();

            var valueToReplaceDto = _mapper.Map<ValueToReplaceDto>(valueToReplace);
            return createValueToReplaceCommand.Controller.CreatedAtRoute("GetValueToReplace",
                new { valueToReplaceDto.ValueToReplaceId },
                valueToReplaceDto);
        }
    }
}
