using Application.Authentication.Dto;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Authentication.Commands
{
    public class ResetPasswordCommand
    {
        public class Request : IRequest<PasswordChangeResponseDto>
        {
            public ResetPasswordDto ResetPassword { get; set; }
            public Request(ResetPasswordDto resetPassword) => ResetPassword = resetPassword;
        }

        public class Handler : IRequestHandler<Request, PasswordChangeResponseDto>
        {
            private readonly UserManager<ApplicationUser> _userManager;
            public Handler(UserManager<ApplicationUser> userManager) => _userManager = userManager;

            public async Task<PasswordChangeResponseDto> Handle(Request request, CancellationToken cancellationToken)
            {
                if (request.ResetPassword.Password != request.ResetPassword.ConfirmPassword)
                    return PasswordChangeResponseDto.PasswordsDoNotMatch(request.ResetPassword.Email);

                var user = await _userManager.FindByEmailAsync(request.ResetPassword.Email);
                if (user is null) return PasswordChangeResponseDto.BadRequest(request.ResetPassword.Email);

                var resetPassResult = await _userManager.ResetPasswordAsync(user, request.ResetPassword.Token, request.ResetPassword.Password);
                if (!resetPassResult.Succeeded) return PasswordChangeResponseDto.TokenError(request.ResetPassword.Email);

                return PasswordChangeResponseDto.PasswordResetSuccess(request.ResetPassword.Email);
            }
        }
    }
}