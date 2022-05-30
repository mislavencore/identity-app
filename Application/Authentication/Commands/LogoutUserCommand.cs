using Application.Authentication.Dto;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Authentication.Commands
{
    public class LogoutUserCommand
    {
        public class Request : IRequest<ApplicationUserDto>
        {
            public string UserName { get; set; }
            public Request(string userName) => UserName = userName;
        }

        public class Handler : IRequestHandler<Request, ApplicationUserDto>
        {
            private readonly IMapper _mapper;
            private readonly UserManager<ApplicationUser> _userManager;

            public Handler(UserManager<ApplicationUser> userManager, IMapper mapper)
            {
                _userManager = userManager;
                _mapper = mapper;
            }

            public async Task<ApplicationUserDto> Handle(Request request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByNameAsync(request.UserName);
                if (user == null) return null;

                user.RefreshToken = null;
                await _userManager.UpdateAsync(user);

                return _mapper.Map<ApplicationUserDto>(user);
            }
        }
    }
}