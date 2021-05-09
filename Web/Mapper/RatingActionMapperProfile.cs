using AutoMapper;
using BulletinBoardAPI.Models.Realizations;
using Web.DTO.Rating;

namespace Web.Mapper
{
    public class RatingActionMapperProfile : Profile
    {
        public RatingActionMapperProfile()
        {
            CreateMap<RatingActionDto, RatingAction>()
                .ForMember(dest => dest.AdId, opt => opt.MapFrom(src => src.AdId))
                .ForAllOtherMembers(opt => opt.UseDestinationValue());
        }
    }
}
