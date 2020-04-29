using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Foundation.Api.Data.Entities;
using Foundation.Api.Mediator.Queries;
using Foundation.Api.Mediator.Responses;
using Foundation.Api.Models;
using Foundation.Api.Models.Pagination;
using Foundation.Api.Services;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace Foundation.Api.Mediator.Handlers
{
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

            //var previousPageLink = valueToReplacesFromRepo.HasPrevious
            //        ? CreateValueToReplacesResourceUri(request.ValueToReplaceParametersDto,
            //            ResourceUriType.PreviousPage)
            //        : null;

            //var nextPageLink = valueToReplacesFromRepo.HasNext
            //    ? CreateValueToReplacesResourceUri(request.ValueToReplaceParametersDto,
            //        ResourceUriType.NextPage)
            //    : null;

            var paginationMetadata = new
            {
                totalCount = valueToReplacesFromRepo.TotalCount,
                pageSize = valueToReplacesFromRepo.PageSize,
                pageNumber = valueToReplacesFromRepo.PageNumber,
                totalPages = valueToReplacesFromRepo.TotalPages,
                hasPrevious = valueToReplacesFromRepo.HasPrevious,
                hasNext = valueToReplacesFromRepo.HasNext,
                //previousPageLink = "",
                //nextPageLink = ""
            };

            var paginationHeader = new KeyValuePair<string, StringValues>("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            var response = new GetAllValueToReplacesQueryResponse()
            {
                ValueToReplaceDtoIEnumerable = _mapper.Map<IEnumerable<ValueToReplaceDto>>(valueToReplacesFromRepo),
                ResponseHeaderPagination = paginationHeader
            };

            return response;
        }


        private string CreateValueToReplacesResourceUri(
            ValueToReplaceParametersDto valueToReplaceParametersDto,
            ResourceUriType type)
        {
            return "";
            //switch (type)
            //{
            //    case ResourceUriType.PreviousPage:
            //        return Url.Link("GetValueToReplaces",
            //            new
            //            {
            //                filters = valueToReplaceParametersDto.Filters,
            //                orderBy = valueToReplaceParametersDto.SortOrder,
            //                pageNumber = valueToReplaceParametersDto.PageNumber - 1,
            //                pageSize = valueToReplaceParametersDto.PageSize,
            //                searchQuery = valueToReplaceParametersDto.QueryString
            //            });
            //    case ResourceUriType.NextPage:
            //        return Url.Link("GetValueToReplaces",
            //            new
            //            {
            //                filters = valueToReplaceParametersDto.Filters,
            //                orderBy = valueToReplaceParametersDto.SortOrder,
            //                pageNumber = valueToReplaceParametersDto.PageNumber + 1,
            //                pageSize = valueToReplaceParametersDto.PageSize,
            //                searchQuery = valueToReplaceParametersDto.QueryString
            //            });

            //    default:
            //        return Url.Link("GetValueToReplaces",
            //            new
            //            {
            //                filters = valueToReplaceParametersDto.Filters,
            //                orderBy = valueToReplaceParametersDto.SortOrder,
            //                pageNumber = valueToReplaceParametersDto.PageNumber,
            //                pageSize = valueToReplaceParametersDto.PageSize,
            //                searchQuery = valueToReplaceParametersDto.QueryString
            //            });
            //}
        }
    }
}
