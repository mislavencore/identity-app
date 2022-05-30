using Application.Authentication.Dto;
using Domain.Entities;
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
    public class LoginVerificationCommand
    {
        public class Request : IRequest<object>
        {
            public TwoStepAuthenticationDto Model { get; set; }
            public Request(TwoStepAuthenticationDto model) => Model = model;
        }

        public class Handler : IRequestHandler<Request, object>
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly IConfiguration _configuration;
            private readonly IWebTokenService _tokenService;

            public Handler(UserManager<ApplicationUser> userManager, IConfiguration configuration, IWebTokenService tokenService)
            {
                _userManager = userManager;
                _configuration = configuration;
                _tokenService = tokenService;
            }

            public async Task<object> Handle(Request request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByEmailAsync(request.Model.Email);
                if (user == null) return TokenModelStatusDto.InvalidClient();

                var validVerification = await _userManager.VerifyTwoFactorTokenAsync(user, request.Model.Provider, request.Model.Token);
                if (!validVerification) return TokenModelStatusDto.Invalid2FATokenValidation();

                await _userManager.ResetAuthenticatorKeyAsync(user);

                var token = _tokenService.CreateToken(user.UserName, isAdmin: true);
                int refreshTokenValidityInDays = int.Parse(_configuration["JWT:RefreshTokenValidityInDays"]);

                user.RefreshToken = _tokenService.GenerateRefreshToken();
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

                await _userManager.UpdateAsync(user);

                var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
                return TokenModelStatusDto.Success(accessToken, user.RefreshToken, true);
            }
        }
    }
}