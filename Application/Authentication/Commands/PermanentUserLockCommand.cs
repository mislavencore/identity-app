using Application.Authentication.Dto;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Authentication.Commands
{
    public class PermanentUserLockCommand
    {
        public class Request : IRequest<ApplicationUserDto>
        {
            public string UserId { get; set; }
            public Request(string userId) => UserId = userId;
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
                var user = await _userManager.FindByIdAsync(request.UserId);
                await _userManager.SetLockoutEndDateAsync(user, DateTime.MaxValue);

                return _mapper.Map<ApplicationUserDto>(user);
            }
        }
    }
}