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

    public class DeleteValueToReplaceHandler : IRequestHandler<DeleteValueToReplaceCommand, bool>
    {
        private readonly IValueToReplaceRepository _valueToReplaceRepository;
        private readonly IMapper _mapper;

        public DeleteValueToReplaceHandler(IValueToReplaceRepository valueToReplaceRepository
            , IMapper mapper)
        {
            _valueToReplaceRepository = valueToReplaceRepository ??
                throw new ArgumentNullException(nameof(valueToReplaceRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<bool> Handle(DeleteValueToReplaceCommand command, CancellationToken cancellationToken)
        {
            var valueToReplaceFromRepo = _valueToReplaceRepository.GetValueToReplace(command.ValueToReplaceId);

            if (valueToReplaceFromRepo == null)
            {
                return false;
            }

            _valueToReplaceRepository.DeleteValueToReplace(valueToReplaceFromRepo);
            _valueToReplaceRepository.Save();

            return true;
        }
    }
}
