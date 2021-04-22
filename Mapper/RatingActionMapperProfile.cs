using AutoMapper;
using BulletinBoardAPI.DTO.Rating;
using BulletinBoardAPI.Models.Realizations;

namespace BulletinBoardAPI.Mapper
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
