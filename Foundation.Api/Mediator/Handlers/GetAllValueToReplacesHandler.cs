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

    public class GetAllValueToReplacesHandler : IRequestHandler<GetAllValueToReplacesQuery, GetAllValueToReplacesQueryResponse> // left is what want want to get in, right is what we want to send out
    {
        private readonly IValueToReplaceRepository _valueToReplaceRepository;
        private readonly IMapper _mapper;

        public GetAllValueToReplacesHandler(IValueToReplaceRepository valueToReplaceRepository
            , IMapper mapper)
        {
            _valueToReplaceRepository = valueToReplaceRepository ??
                throw new ArgumentNullException(nameof(valueToReplaceRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<GetAllValueToReplacesQueryResponse> Handle(GetAllValueToReplacesQuery request, CancellationToken cancellationToken)
        {
            var valueToReplacesFromRepo = _valueToReplaceRepository.GetValueToReplaces(request.ValueToReplaceParametersDto);

            var paginationMetadata = new
            {
                totalCount = valueToReplacesFromRepo.TotalCount,
                pageSize = valueToReplacesFromRepo.PageSize,
                pageNumber = valueToReplacesFromRepo.PageNumber,
                totalPages = valueToReplacesFromRepo.TotalPages,
                hasPrevious = valueToReplacesFromRepo.HasPrevious,
                hasNext = valueToReplacesFromRepo.HasNext,
                previousPageLink = valueToReplacesFromRepo.HasPrevious ? request.PreviousPageLink : null,
                nextPageLink = valueToReplacesFromRepo.HasNext ? request.NextPageLink : null
            };

            var paginationHeader = new KeyValuePair<string, StringValues>("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            var response = new GetAllValueToReplacesQueryResponse()
            {
                ValueToReplaceDtoIEnumerable = _mapper.Map<IEnumerable<ValueToReplaceDto>>(valueToReplacesFromRepo),
                ResponseHeaderPagination = paginationHeader
            };

            return response;
        }
    }
}
