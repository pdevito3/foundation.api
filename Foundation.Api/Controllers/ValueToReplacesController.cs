namespace Foundation.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using AutoMapper;
    using Foundation.Api.Models;
    using Foundation.Api.Services;
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
        public ActionResult<ValueToReplaceDto> GetValueToReplaces()
        {
            var valueToReplaceFromRepo = _valueToReplaceRepository.GetValueToReplaces();

            if (valueToReplaceFromRepo == null)
            {
                return NotFound();
            }

            var valueToReplaceDto = _mapper.Map<IEnumerable<ValueToReplaceDto>>(valueToReplaceFromRepo);

            return Ok(valueToReplaceDto);
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

    }
}