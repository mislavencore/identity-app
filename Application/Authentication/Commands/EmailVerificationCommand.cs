using Application.Authentication.Dto;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Authentication.Commands
{
    public class EmailVerificationCommand
    {
        public class Request : IRequest<GeneralResponseDto>
        {
            public string Email { get; set; }
            public string Token { get; set; }
            public Request(string email, string token)
            {
                Email = email;
                Token = token;
            }
        }

        public class Handler : IRequestHandler<Request, GeneralResponseDto>
        {
            private readonly UserManager<ApplicationUser> _userManager;

            public Handler(UserManager<ApplicationUser> userManager) 
                => _userManager = userManager;

            public async Task<GeneralResponseDto> Handle(Request request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null) return GeneralResponseDto.InvalidRequest();

                var confirmResult = await _userManager.ConfirmEmailAsync(user, request.Token);
                if (!confirmResult.Succeeded)  return GeneralResponseDto.InvalidRequest();

                return GeneralResponseDto.EmailValidationSuccess();
            }
        }
    }
}