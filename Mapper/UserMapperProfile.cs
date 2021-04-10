using AutoMapper;
using BulletinBoardAPI.DTO.User;
using BulletinBoardAPI.Models.Realizations;

namespace BulletinBoardAPI.Mapper
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            CreateMap<UserRegisterDto, User>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username))
                .ForAllOtherMembers(opt => opt.Ignore());
            CreateMap<User, UserGetDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForAllOtherMembers(opt => opt.Ignore());
        }
    }
}
