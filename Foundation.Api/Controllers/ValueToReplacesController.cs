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
        public IActionResult GetCategories([FromQuery] ValueToReplaceParametersDto valueToReplaceParametersDto)
        {
            var query = new GetAllValueToReplacesQuery(valueToReplaceParametersDto, this);
            var result = _mediator.Send(query);

            return result.Result;
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
    }
}