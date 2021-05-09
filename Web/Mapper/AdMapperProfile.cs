using AutoMapper;
using Data.Models.Realizations;
using Web.DTO.Ad;

namespace Web.Mapper
{
    public class AdMapperProfile : Profile
    {
        public AdMapperProfile()
        {
            CreateMap<AdDto, Ad>()
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
                .ForAllOtherMembers(opt => opt.UseDestinationValue());
        }
    }
}
