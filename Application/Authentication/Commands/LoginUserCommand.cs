using Application.Authentication.Dto;
using Domain.Entities;
using Infrastructure.EmailProvider;
using Infrastructure.WebToken;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Authentication.Commands
{
    public class LoginUserCommand
    {
        public class Request : IRequest<TokenModelStatusDto>
        {
            public LoginModelDto Model { get; set; }
            public Request(LoginModelDto model) => Model = model;
        }

        public class Handler : IRequestHandler<Request, TokenModelStatusDto>
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly SignInManager<ApplicationUser> _signInManager;
            private readonly IConfiguration _configuration;
            private readonly IWebTokenService _tokenService;
            private readonly IEmailSender _emailSender;

            public Handler(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, 
                IConfiguration configuration, IWebTokenService tokenService, IEmailSender emailSender)
            {
                _userManager = userManager;
                _signInManager = signInManager;
                _configuration = configuration;
                _tokenService = tokenService;
                _emailSender = emailSender;
            }

            public async Task<TokenModelStatusDto> Handle(Request request, CancellationToken cancellationToken)
            {
                var result = await _signInManager
                    .PasswordSignInAsync(request.Model.Username, request.Model.Password, false, lockoutOnFailure: true);

                if(result.IsLockedOut) return TokenModelStatusDto.Locked();
                if (result.IsNotAllowed) return TokenModelStatusDto.NotAllowed();

                var user = await _userManager.FindByNameAsync(request.Model.Username);

                if (result.RequiresTwoFactor)
                {
                    if (!await _userManager.CheckPasswordAsync(user, request.Model.Password))
                        return TokenModelStatusDto.Unauthorized();

                    var providers = await _userManager.GetValidTwoFactorProvidersAsync(user);
                    if (!providers.Contains("Email"))
                        return TokenModelStatusDto.Invalid2FATokenValidation();

                    await _userManager.ResetAuthenticatorKeyAsync(user);
                    var totpToken = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
                    _emailSender.Send(user.Email, totpToken, totpToken);

                    return TokenModelStatusDto.Success2FA(result.RequiresTwoFactor);
                }

                if (!result.Succeeded) return TokenModelStatusDto.Unauthorized();

                var token = _tokenService.CreateToken(user.UserName);
                int refreshTokenValidityInDays = int.Parse(_configuration["JWT:RefreshTokenValidityInDays"]);

                user.RefreshToken = _tokenService.GenerateRefreshToken();
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

                await _userManager.UpdateAsync(user);
                var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

                return TokenModelStatusDto.Success(accessToken, user.RefreshToken);
            }
        }
    }
}