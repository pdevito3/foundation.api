namespace Foundation.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    using AutoMapper;
    using Foundation.Api.Data.Entities;
    using Foundation.Api.Models.Pagination;
    using Foundation.Api.Models.ValueToReplace;
    using Foundation.Api.Services;
    using Foundation.Api.Services.ValueToReplace;
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/v1/ValueToReplaceLowers")]
    public class ValueToReplacesController : Controller
    {
        private readonly IValueToReplaceRepository _valueToReplaceRepository;
        private readonly IMapper _mapper;

        public ValueToReplacesController(IValueToReplaceRepository valueToReplaceRepository
            , IMapper mapper)
        {
            _valueToReplaceRepository = valueToReplaceRepository ??
                throw new ArgumentNullException(nameof(valueToReplaceRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }


        [HttpGet(Name = "GetValueToReplaces")]
        public ActionResult<IEnumerable<ValueToReplaceDto>> GetCategories([FromQuery] ValueToReplaceParametersDto valueToReplaceParametersDto)
        {
            var valueToReplacesFromRepo = _valueToReplaceRepository.GetValueToReplaces(valueToReplaceParametersDto);
            
            var previousPageLink = valueToReplacesFromRepo.HasPrevious
                    ? CreateValueToReplacesResourceUri(valueToReplaceParametersDto,
                        ResourceUriType.PreviousPage)
                    : null;

            var nextPageLink = valueToReplacesFromRepo.HasNext
                ? CreateValueToReplacesResourceUri(valueToReplaceParametersDto,
                    ResourceUriType.NextPage)
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

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            var valueToReplacesDto = _mapper.Map<IEnumerable<ValueToReplaceDto>>(valueToReplacesFromRepo);
            return Ok(valueToReplacesDto);
        }


        [HttpGet("{valueToReplaceId}", Name = "GetValueToReplace")]
        public ActionResult<ValueToReplaceDto> GetValueToReplace(int valueToReplaceId)
        {
            var valueToReplaceFromRepo = _valueToReplaceRepository.GetValueToReplace(valueToReplaceId);

            if (valueToReplaceFromRepo == null)
            {
                return NotFound();
            }

            var valueToReplaceDto = _mapper.Map<ValueToReplaceDto>(valueToReplaceFromRepo);

            return Ok(valueToReplaceDto);
        }

        [HttpPost]
        public ActionResult<ValueToReplaceDto> AddValueToReplace(ValueToReplaceForCreationDto valueToReplaceForCreation)
        {
            var valueToReplace = _mapper.Map<ValueToReplace>(valueToReplaceForCreation);
            _valueToReplaceRepository.AddValueToReplace(valueToReplace);
            _valueToReplaceRepository.Save();

            var valueToReplaceDto = _mapper.Map<ValueToReplaceDto>(valueToReplace);
            return CreatedAtRoute("GetValueToReplace",
                new { valueToReplaceDto.ValueToReplaceId },
                valueToReplaceDto);
        }

        [HttpDelete("{valueToReplaceId}")]
        public ActionResult DeleteValueToReplace(int valueToReplaceId)
        {
            var valueToReplaceFromRepo = _valueToReplaceRepository.GetValueToReplace(valueToReplaceId);

            if (valueToReplaceFromRepo == null)
            {
                return NotFound();
            }

            _valueToReplaceRepository.DeleteValueToReplace(valueToReplaceFromRepo);
            _valueToReplaceRepository.Save();

            return NoContent();
        }

        [HttpPut("{valueToReplaceId}")]
        public IActionResult UpdateValueToReplace(int valueToReplaceId, ValueToReplaceForUpdateDto valueToReplace)
        {
            var valueToReplaceFromRepo = _valueToReplaceRepository.GetValueToReplace(valueToReplaceId);

            if (valueToReplaceFromRepo == null)
            {
                return NotFound();
            }

            _mapper.Map(valueToReplace, valueToReplaceFromRepo);
            _valueToReplaceRepository.UpdateValueToReplace(valueToReplaceFromRepo);

            _valueToReplaceRepository.Save();

            return NoContent();
        }

        [HttpPatch("{valueToReplaceId}")]
        public IActionResult PartiallyUpdateValueToReplace(int valueToReplaceId, JsonPatchDocument<ValueToReplaceForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var existingValueToReplace = _valueToReplaceRepository.GetValueToReplace(valueToReplaceId);

            if (existingValueToReplace == null)
            {
                return NotFound();
            }

            var valueToReplaceToPatch = _mapper.Map<ValueToReplaceForUpdateDto>(existingValueToReplace); // map the valueToReplace we got from the database to an updatable valueToReplace model
            patchDoc.ApplyTo(valueToReplaceToPatch, ModelState); // apply patchdoc updates to the updatable valueToReplace

            if (!TryValidateModel(valueToReplaceToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(valueToReplaceToPatch, existingValueToReplace); // apply updates from the updatable valueToReplace to the db entity so we can apply the updates to the database
            _valueToReplaceRepository.UpdateValueToReplace(existingValueToReplace); // apply business updates to data if needed

            _valueToReplaceRepository.Save(); // save changes in the database

            return NoContent();
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