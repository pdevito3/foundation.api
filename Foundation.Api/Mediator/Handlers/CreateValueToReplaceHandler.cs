namespace Foundation.Api.Mediator.Handlers
{
    using AutoMapper;
    using Foundation.Api.Data.Entities;
    using Foundation.Api.Mediator.Commands;
    using Foundation.Api.Models;
    using Foundation.Api.Services;
    using MediatR;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class CreateValueToReplaceHandler : IRequestHandler<CreateValueToReplaceCommand, ValueToReplaceDto>
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

        public async Task<ValueToReplaceDto> Handle(CreateValueToReplaceCommand valueForToReplaceForCreationDto, CancellationToken cancellationToken)
        {
            var valueToReplace = _mapper.Map<ValueToReplace>(valueForToReplaceForCreationDto);
            _valueToReplaceRepository.AddValueToReplace(valueToReplace);
            _valueToReplaceRepository.Save();

            return _mapper.Map<ValueToReplaceDto>(valueToReplace);
        }
    }
}
