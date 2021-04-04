using System;
using AutoMapper;
using BulletinBoardAPI.DTO;
using BulletinBoardAPI.Models.Realizations;

namespace BulletinBoardAPI.Mapper
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            CreateMap<UserDto, User>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
        }
    }
}
