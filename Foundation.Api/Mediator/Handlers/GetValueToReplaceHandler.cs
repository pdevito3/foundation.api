namespace Foundation.Api.Mediator.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Foundation.Api.Mediator.Queries;
    using Foundation.Api.Mediator.Responses;
    using Foundation.Api.Models;
    using Foundation.Api.Services;
    using MediatR;
    using Microsoft.Extensions.Primitives;

    public class GetValueToReplaceHandler : IRequestHandler<GetValueToReplaceQuery, ValueToReplaceDto>
    {
        private readonly IValueToReplaceRepository _valueToReplaceRepository;
        private readonly IMapper _mapper;

        public GetValueToReplaceHandler(IValueToReplaceRepository valueToReplaceRepository
            , IMapper mapper)
        {
            _valueToReplaceRepository = valueToReplaceRepository ??
                throw new ArgumentNullException(nameof(valueToReplaceRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ValueToReplaceDto> Handle(GetValueToReplaceQuery request, CancellationToken cancellationToken)
        {
            var valueToReplaceFromRepo = _valueToReplaceRepository.GetValueToReplace(request.ValueToReplaceId);

            if (valueToReplaceFromRepo == null)
            {
                return null;
            }

            var valueToReplaceDto = _mapper.Map<ValueToReplaceDto>(valueToReplaceFromRepo);

            return valueToReplaceDto;
        }
    }
}
