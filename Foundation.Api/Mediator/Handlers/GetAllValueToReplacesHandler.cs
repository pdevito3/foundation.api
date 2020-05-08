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
    using Foundation.Api.Models.Pagination;
    using Foundation.Api.Services;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Primitives;

    public class GetAllValueToReplacesHandler : IRequestHandler<GetAllValueToReplacesQuery, IActionResult> // left is what want want to get in, right is what we want to send out
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

        public async Task<IActionResult> Handle(GetAllValueToReplacesQuery request, CancellationToken cancellationToken)
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

            var paginationMetadata = new
            {
                totalCount = valueToReplacesFromRepo.TotalCount,
                pageSize = valueToReplacesFromRepo.PageSize,
                pageNumber = valueToReplacesFromRepo.PageNumber,
                totalPages = valueToReplacesFromRepo.TotalPages,
                hasPrevious = valueToReplacesFromRepo.HasPrevious,
                hasNext = valueToReplacesFromRepo.HasNext,
                previousPageLink,
                nextPageLink
            };

            request.Controller.Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            var returnableValueToReplace = _mapper.Map<IEnumerable<ValueToReplaceDto>>(valueToReplacesFromRepo);

            return request.Controller.Ok(returnableValueToReplace);
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
