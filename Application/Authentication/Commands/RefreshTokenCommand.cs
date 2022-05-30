using Application.Authentication.Dto;
using Domain.Entities;
using Infrastructure.WebToken;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Authentication.Commands
{
    public class RefreshTokenCommand
    {
        public class Request : IRequest<TokenModelStatusDto>
        {
            public TokenModelDto TokenModel { get; set; }
            public Request(TokenModelDto tokenModel) => TokenModel = tokenModel;
        }

        public class Handler : IRequestHandler<Request, TokenModelStatusDto>
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly IWebTokenService _tokenService;

            public Handler(UserManager<ApplicationUser> userManager, IWebTokenService tokenService)
            {
                _userManager = userManager;
                _tokenService = tokenService;
            }
            
            public async Task<TokenModelStatusDto> Handle(Request request, CancellationToken cancellationToken)
            {
                if (request.TokenModel is null) TokenModelStatusDto.InvalidClient();

                string? accessToken = request.TokenModel.Token;
                string? refreshToken = request.TokenModel.RefreshToken;

                var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);

                if (principal == null) TokenModelStatusDto.InvalidToken();

                string username = principal.Identity.Name;
                var user = await _userManager.FindByNameAsync(username);

                if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                    TokenModelStatusDto.InvalidToken();

                var newAccessToken = _tokenService.CreateToken(principal.Identity.Name, principal.IsInRole("Admin"));

                user.RefreshToken = _tokenService.GenerateRefreshToken();
                await _userManager.UpdateAsync(user);

                var writtenAccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken);
                return TokenModelStatusDto.Success(writtenAccessToken, user.RefreshToken);
            }
        }
    }
}