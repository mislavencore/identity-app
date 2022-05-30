using Application.Authentication.Dto;
using Domain.Entities;
using Infrastructure.EmailProvider;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Application.Authentication.Commands
{
    public class ForgotPasswordCommand
    {
        public class Request : IRequest<PasswordChangeResponseDto>
        {
            public ForgotPasswordDto ForgotPasswordDto { get; set; }
            public Request(ForgotPasswordDto forgotPasswordDto) => ForgotPasswordDto = forgotPasswordDto;
        }

        public class Handler : IRequestHandler<Request, PasswordChangeResponseDto>
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly IConfiguration _configuration;
            private readonly IEmailSender _emailSender;

            public Handler(UserManager<ApplicationUser> userManager, IConfiguration configuration, IEmailSender emailSender)
            {
                _userManager = userManager;
                _configuration = configuration;
                _emailSender = emailSender;
            }

            public async Task<PasswordChangeResponseDto> Handle(Request request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByEmailAsync(request.ForgotPasswordDto.Email);
                
                if (user is null) return PasswordChangeResponseDto.BadRequest(request.ForgotPasswordDto.Email);
                if (user.PasswordHash is null) return PasswordChangeResponseDto.ExternalUser(request.ForgotPasswordDto.Email);

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var url = _configuration["URL:ForgottenPassword"] + "?token=" + HttpUtility.UrlEncode(token) + "&email=" + request.ForgotPasswordDto.Email;
                _emailSender.Send(request.ForgotPasswordDto.Email, "Password change", url);

                return PasswordChangeResponseDto.ForgottenPasswordSuccess(request.ForgotPasswordDto.Email);
            }
        }
    }
}