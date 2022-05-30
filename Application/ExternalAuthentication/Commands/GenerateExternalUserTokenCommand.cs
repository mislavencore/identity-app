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

namespace Application.ExternalAuthentication.Commands
{
    public class GenerateExternalUserTokenCommand
    {
        public class Request : IRequest<TokenModelStatusDto>
        {
            public ApplicationUserDto User { get; set; }
            public Request(ApplicationUserDto user) => User = user;
        }

        public class Handler : IRequestHandler<Request, TokenModelStatusDto>
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

            public async Task<TokenModelStatusDto> Handle(Request request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByNameAsync(request.User.UserName);
                if (user == null) return TokenModelStatusDto.InvalidClient();

                var token = _tokenService.CreateToken(user.UserName);
                int refreshTokenValidityInDays = int.Parse(_configuration["JWT:RefreshTokenValidityInDays"]);

                user.RefreshToken = _tokenService.GenerateRefreshToken();
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

                await _userManager.UpdateAsync(user);

                var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
                return TokenModelStatusDto.Success(accessToken, user.RefreshToken, false);
            }
        }
    }
}