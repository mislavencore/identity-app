using Application.Authentication.Dto;
using AutoMapper;
using Domain.Entities;

namespace Application.Authentication
{
    public class AuthenticationMappingProfile : Profile
    {
        public AuthenticationMappingProfile()
        {
            CreateMap<ApplicationUser, ApplicationUserDto>();
            CreateMap<ApplicationUserDto, ApplicationUser>();
        }
    }
}