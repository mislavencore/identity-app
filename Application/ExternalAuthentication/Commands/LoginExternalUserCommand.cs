using Application.Authentication.Dto;
using Application.ExternalAuthentication.Dto;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ExternalAuthentication.Commands
{
    public class LoginExternalUserCommand
    {
        public class Request : IRequest<ApplicationUserDto>
        {
            public string Provider { get; set; }
            public ExternalAuthenticationPayloadDto GooglePayload { get; set; }
            public Request(string provider, ExternalAuthenticationPayloadDto googlePayload)
            {
                Provider = provider;
                GooglePayload = googlePayload;
            }
        }

        public class Handler : IRequestHandler<Request, ApplicationUserDto>
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly IMapper _mapper;

            public Handler(UserManager<ApplicationUser> userManager, IMapper mapper)
            {
                _userManager = userManager;
                _mapper = mapper;
            }

            public async Task<ApplicationUserDto> Handle(Request request, CancellationToken cancellationToken)
            {
                var info = new UserLoginInfo(request.Provider, request.GooglePayload.Subject, request.Provider);
                
                var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
                if (user != null) return _mapper.Map<ApplicationUserDto>(user);
                
                user = await _userManager.FindByEmailAsync(request.GooglePayload.Email);
                if (user != null) await _userManager.AddLoginAsync(user, info);
                
                user = new ApplicationUser { Email = request.GooglePayload.Email,  UserName = request.GooglePayload.Email };

                await _userManager.CreateAsync(user);
                await _userManager.AddLoginAsync(user, info);
                return _mapper.Map<ApplicationUserDto>(user);
            }
        }
    }
}