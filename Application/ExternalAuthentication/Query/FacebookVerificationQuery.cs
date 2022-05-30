using Application.ExternalAuthentication.Dto;
using Application.Constants;
using AutoMapper;
using Infrastructure.ExternalRequests;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ExternalAuthentication.Query
{
    public class FacebookVerificationQuery
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
            private readonly IExternalRequests _externalRequests;

            public Handler(IConfiguration configuration, IMapper mapper, IExternalRequests externalRequests)
            {
                _configuration = configuration;
                _mapper = mapper;
                _externalRequests = externalRequests;
            }

            public async Task<ExternalAuthenticationPayloadDto> Handle(Request request, CancellationToken cancellationToken)
            {
                var validateTokenUrl = string.Format(FacebookConstants.TokenValidationUrl, request.Token,
                    _configuration["FacebookAuthSettings:AppId"], _configuration["FacebookAuthSettings:AppSecret"]);

                var validationResponse = await _externalRequests.GetRequestAsync(validateTokenUrl);
                var validatedTokenResult = JsonConvert.DeserializeObject<FacebookTokenValidationDto>(validationResponse);

                if (!validatedTokenResult.Data.IsValid) return null;

                var getUserInfoUrl = string.Format(FacebookConstants.UserInfoUrl, request.Token);
                var dataResponse = await _externalRequests.GetRequestAsync(getUserInfoUrl);
                var userInfo = JsonConvert.DeserializeObject<FacebookUserInfoDto>(dataResponse);

                return _mapper.Map<ExternalAuthenticationPayloadDto>(userInfo);
            }
        }
    }
}