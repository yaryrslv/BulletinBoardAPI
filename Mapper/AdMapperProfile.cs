using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BulletinBoardAPI.DTO;
using BulletinBoardAPI.Models.Realizations;

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
