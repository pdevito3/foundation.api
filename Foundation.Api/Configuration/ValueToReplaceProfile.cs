namespace Foundation.Api.Configuration
{
    using AutoMapper;
    using Foundation.Api.Data.Entities;
    using Foundation.Api.Mediator.Commands;
    using Foundation.Api.Models;

    public class ValueToReplaceProfile : Profile
    {
        public ValueToReplaceProfile()
        {
            //createmap<to this, from this>
            CreateMap<ValueToReplace, ValueToReplaceDto>()
                .ReverseMap();
            CreateMap<ValueToReplace, CreateValueToReplaceCommand>()
                .ReverseMap();            
            CreateMap<ValueToReplaceForCreationDto, ValueToReplace>()
                .ReverseMap();
            CreateMap<ValueToReplaceForUpdateDto, ValueToReplace>()
                .ReverseMap();
        }
    }
}