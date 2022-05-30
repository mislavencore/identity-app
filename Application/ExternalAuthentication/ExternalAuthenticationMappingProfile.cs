using Application.ExternalAuthentication.Dto;
using AutoMapper;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace Application.AuthenticationExternal
{
    public class ExternalAuthenticationMappingProfile : Profile
    {
        public ExternalAuthenticationMappingProfile()
        {
            CreateMap<Payload, ExternalAuthenticationPayloadDto>();

            CreateMap<FacebookUserInfoDto, ExternalAuthenticationPayloadDto>()
                .ForMember(x => x.Subject, y => y.MapFrom(z => z.Id));
        }
    }
}