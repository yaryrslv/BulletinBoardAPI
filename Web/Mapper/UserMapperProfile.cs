using AutoMapper;
using BulletinBoardAPI.Models.Realizations;
using Web.DTO.User;

namespace Web.Mapper
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            CreateMap<UserRegisterDto, User>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.PhoneNumberConfirmed, opt => opt.MapFrom(src => true))
                .ForAllOtherMembers(opt => opt.Ignore());
            CreateMap<User, UserGetDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForAllOtherMembers(opt => opt.Ignore());
        }
    }
}
