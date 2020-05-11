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

    public class UpdateEntireValueToReplaceHandler : IRequestHandler<UpdateEntireValueToReplaceCommand, string>
    {
        private readonly IValueToReplaceRepository _valueToReplaceRepository;
        private readonly IMapper _mapper;

        public UpdateEntireValueToReplaceHandler(IValueToReplaceRepository valueToReplaceRepository
            , IMapper mapper)
        {
            _valueToReplaceRepository = valueToReplaceRepository ??
                throw new ArgumentNullException(nameof(valueToReplaceRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<string> Handle(UpdateEntireValueToReplaceCommand updateEntireValueToReplaceCommand, CancellationToken cancellationToken)
        {
            var valueToReplaceFromRepo = _valueToReplaceRepository.GetValueToReplace(updateEntireValueToReplaceCommand.ValueToReplaceId);

            if (valueToReplaceFromRepo == null)
            {
                return "NotFound";
            }

            _mapper.Map(updateEntireValueToReplaceCommand.ValueToReplaceForUpdateDto, valueToReplaceFromRepo);
            _valueToReplaceRepository.UpdateValueToReplace(valueToReplaceFromRepo);

            _valueToReplaceRepository.Save();

            return "NoContent";
        }
    }
}
