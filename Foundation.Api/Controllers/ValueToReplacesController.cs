namespace Foundation.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using Foundation.Api.Mediator.Commands;
    using Foundation.Api.Mediator.Queries;
    using Foundation.Api.Models;
    using Foundation.Api.Models.Pagination;
    using MediatR;
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/v1/ValueToReplaceLowers")]
    public class ValueToReplacesController : Controller
    {
        private readonly IMediator _mediator;

        public ValueToReplacesController(IMediator mediator)
        {
            _mediator = mediator ??
                throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet(Name = "GetValueToReplaces")]
        public ActionResult<IEnumerable<ValueToReplaceDto>> GetCategories([FromQuery] ValueToReplaceParametersDto valueToReplaceParametersDto)
        {
            var previousPageLink = CreateValueToReplacesResourceUri(valueToReplaceParametersDto, ResourceUriType.PreviousPage);
            var nextPageLink = CreateValueToReplacesResourceUri(valueToReplaceParametersDto, ResourceUriType.NextPage);

            var query = new GetAllValueToReplacesQuery(valueToReplaceParametersDto, previousPageLink, nextPageLink);
            var result = _mediator.Send(query);

            Response.Headers.Add(result.Result.ResponseHeaderPagination);

            return Ok(result.Result.ValueToReplaceDtoIEnumerable);
        }

        [HttpGet("{valueToReplaceId}", Name = "GetValueToReplace")]
        public IActionResult GetValueToReplace(int valueToReplaceId)
        {
            var query = new GetValueToReplaceQuery(valueToReplaceId);
            var result = _mediator.Send(query);

            return result.Result != null ? (IActionResult) Ok(result.Result) : NotFound();
        }

        [HttpPost]
        public ActionResult<ValueToReplaceDto> AddValueToReplace(CreateValueToReplaceCommand command)
        {
            var result = _mediator.Send(command);
            return CreatedAtRoute("GetValueToReplace",
                new { result.Result.ValueToReplaceId },
                result.Result);
        }

        [HttpDelete("{valueToReplaceId}")]
        public IActionResult DeleteValueToReplace(int valueToReplaceId)
        {
            var query = new DeleteValueToReplaceCommand(valueToReplaceId);
            var result = _mediator.Send(query);

            return result.Result ? (IActionResult)NoContent() : NotFound();
        }

        [HttpPut("{valueToReplaceId}")]
        public IActionResult UpdateValueToReplace(int valueToReplaceId, ValueToReplaceForUpdateDto valueToReplace)
        {
            var query = new UpdateEntireValueToReplaceCommand(valueToReplaceId, valueToReplace);
            var result = _mediator.Send(query);

            return result.Result.ToUpper() == "NOCONTENT" ? (IActionResult)NoContent() : NotFound();
        }

        [HttpPatch("{valueToReplaceId}")]
        public IActionResult PartiallyUpdateValueToReplace(int valueToReplaceId, JsonPatchDocument<ValueToReplaceForUpdateDto> patchDoc)
        {
            var query = new UpdatePartialValueToReplaceCommand(valueToReplaceId, patchDoc, this);
            var result = _mediator.Send(query);

            return result.Result;
        }

        private string CreateValueToReplacesResourceUri(
            ValueToReplaceParametersDto valueToReplaceParametersDto,
            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link("GetValueToReplaces",
                        new
                        {
                            filters = valueToReplaceParametersDto.Filters,
                            orderBy = valueToReplaceParametersDto.SortOrder,
                            pageNumber = valueToReplaceParametersDto.PageNumber - 1,
                            pageSize = valueToReplaceParametersDto.PageSize,
                            searchQuery = valueToReplaceParametersDto.QueryString
                        });
                case ResourceUriType.NextPage:
                    return Url.Link("GetValueToReplaces",
                        new
                        {
                            filters = valueToReplaceParametersDto.Filters,
                            orderBy = valueToReplaceParametersDto.SortOrder,
                            pageNumber = valueToReplaceParametersDto.PageNumber + 1,
                            pageSize = valueToReplaceParametersDto.PageSize,
                            searchQuery = valueToReplaceParametersDto.QueryString
                        });

                default:
                    return Url.Link("GetValueToReplaces",
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