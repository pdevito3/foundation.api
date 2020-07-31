namespace Application.Mappings
{
    using Application.Dtos.ValueToReplace;
    using AutoMapper;
    using Domain.Entities;

    public class ValueToReplaceProfile : Profile
    {
        public ValueToReplaceProfile()
        {
            //createmap<to this, from this>
            CreateMap<ValueToReplace, ValueToReplaceDto>()
                .ReverseMap();
            CreateMap<ValueToReplaceForCreationDto, ValueToReplace>();
            CreateMap<ValueToReplaceForUpdateDto, ValueToReplace>()
                .ReverseMap();
        }
    }
}