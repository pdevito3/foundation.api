namespace Foundation.Api.Mediator.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Foundation.Api.Mediator.Queries;
    using Foundation.Api.Models;
    using Foundation.Api.Models.Pagination;
    using Foundation.Api.Services;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    public class GetAllValueToReplacesHandler : IRequestHandler<GetAllValueToReplacesQuery, GetAllValueToReplaceQueryResponse> // left is what want want to get in, right is what we want to send out
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

        public async Task<GetAllValueToReplaceQueryResponse> Handle(GetAllValueToReplacesQuery request, CancellationToken cancellationToken)
        {
            var valueToReplacesFromRepo = _valueToReplaceRepository.GetValueToReplaces(request.ValueToReplaceParametersDto);

            var previousPageLink = valueToReplacesFromRepo.HasPrevious
                    ? CreateValueToReplacesResourceUri(request.ValueToReplaceParametersDto,
                        ResourceUriType.PreviousPage,
                        request.Controller)
                    : null;

            var nextPageLink = valueToReplacesFromRepo.HasNext
                ? CreateValueToReplacesResourceUri(request.ValueToReplaceParametersDto,
                    ResourceUriType.NextPage,
                    request.Controller)
                : null;

            var paginationMetadata = new PaginationHeader
            {
                TotalCount = valueToReplacesFromRepo.TotalCount,
                PageSize = valueToReplacesFromRepo.PageSize,
                PageNumber = valueToReplacesFromRepo.PageNumber,
                TotalPages = valueToReplacesFromRepo.TotalPages,
                HasPrevious = valueToReplacesFromRepo.HasPrevious,
                HasNext = valueToReplacesFromRepo.HasNext,
                PreviousPageLink = previousPageLink,
                NextPageLink = nextPageLink
            };

            var returnableValueToReplace = _mapper.Map<IEnumerable<ValueToReplaceDto>>(valueToReplacesFromRepo);

            var response = new GetAllValueToReplaceQueryResponse { PaginationMetadata = paginationMetadata, ValueToReplaceDtoList = returnableValueToReplace };
            return response;
        }

        private string CreateValueToReplacesResourceUri(
            ValueToReplaceParametersDto valueToReplaceParametersDto,
            ResourceUriType type,
            Controller controller)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return controller.Url.Link("GetValueToReplaces",
                        new
                        {
                            filters = valueToReplaceParametersDto.Filters,
                            orderBy = valueToReplaceParametersDto.SortOrder,
                            pageNumber = valueToReplaceParametersDto.PageNumber - 1,
                            pageSize = valueToReplaceParametersDto.PageSize,
                            searchQuery = valueToReplaceParametersDto.QueryString
                        });
                case ResourceUriType.NextPage:
                    return controller.Url.Link("GetValueToReplaces",
                        new
                        {
                            filters = valueToReplaceParametersDto.Filters,
                            orderBy = valueToReplaceParametersDto.SortOrder,
                            pageNumber = valueToReplaceParametersDto.PageNumber + 1,
                            pageSize = valueToReplaceParametersDto.PageSize,
                            searchQuery = valueToReplaceParametersDto.QueryString
                        });

                default:
                    return controller.Url.Link("GetValueToReplaces",
                        new
                        {
                            filters = valueToReplaceParametersDto.Filters,
                            orderBy = valueToReplaceParametersDto.SortOrder,
                            pageNumber = valueToReplaceParametersDto.PageNumber,
                            pageSize = valueToReplaceParametersDto.PageSize,
                            searchQuery = valueToReplaceParametersDto.QueryString
                        });
            }
        }
    }
}
