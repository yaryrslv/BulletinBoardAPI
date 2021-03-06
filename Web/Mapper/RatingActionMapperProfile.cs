using AutoMapper;
using Data.Models.Realizations;
using Web.DTO.RatinAction;

namespace Web.Mapper
{
    public class RatingActionMapperProfile : Profile
    {
        public RatingActionMapperProfile()
        {
            CreateMap<RatingActionDto, RatingAction>()
                .ForMember(dest => dest.AdId, opt => opt.MapFrom(src => src.AdId))
                .ForAllOtherMembers(opt => opt.UseDestinationValue());
            CreateMap<RatingAction, RatingActionFullDto>();
            CreateMap<RatingActionFullDto, RatingAction>();
            CreateMap<RatingActionDto, RatingActionFullDto>();
        }
    }
}
