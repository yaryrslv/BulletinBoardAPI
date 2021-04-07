using AutoMapper;
using BulletinBoardAPI.DTO.Ad;
using BulletinBoardAPI.Models.Realizations;
using Microsoft.AspNetCore.DataProtection;

namespace BulletinBoardAPI.Mapper
{
    public class AdMapperProfile : Profile
    {
        public AdMapperProfile()
        {
            CreateMap<AdDto, Ad>()
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
                .ForAllOtherMembers(opt => opt.UseDestinationValue());
        }
    }
}
