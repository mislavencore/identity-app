using Application.ExternalAuthentication.Dto;
using AutoMapper;
using Google.Apis.Auth;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace Application.ExternalAuthentication.Query
{
    public class GoogleVerificationQuery
    {
        public class Request : IRequest<ExternalAuthenticationPayloadDto>
        {
            public string Token { get; set; }
            public Request(string token) => Token = token;
        }

        public class Handler : IRequestHandler<Request, ExternalAuthenticationPayloadDto>
        {
            private readonly IConfiguration _configuration;
            private readonly IMapper _mapper;

            public Handler(IConfiguration configuration, IMapper mapper)
            {
                _configuration = configuration;
                _mapper = mapper;
            }

            public async Task<ExternalAuthenticationPayloadDto> Handle(Request request, CancellationToken cancellationToken)
            {
                Payload payload;

                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string>() { _configuration["GoogleAuthSettings:ClientId"] }
                };

                try
                {
                    payload = await GoogleJsonWebSignature.ValidateAsync(request.Token, settings);
                }
                catch
                {
                    return null;
                }

                return _mapper.Map<ExternalAuthenticationPayloadDto>(payload);
            }
        }
    }
}