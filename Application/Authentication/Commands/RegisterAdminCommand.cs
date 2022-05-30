using Application.Authentication.Dto;
using Application.Constants;
using Domain.Entities;
using Infrastructure.EmailProvider;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Application.Authentication.Commands
{
    public class RegisterAdminCommand
    {
        public class Request : IRequest<GeneralResponseDto>
        {
            public RegisterModelDto Model { get; set; }
            public Request(RegisterModelDto model) => Model = model;
        }

        public class Handler : IRequestHandler<Request, GeneralResponseDto>
        {
            private readonly RoleManager<IdentityRole> _roleManager;
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly IConfiguration _configuration;
            private readonly IEmailSender _emailSender;

            public Handler(UserManager<ApplicationUser> userManager,
                RoleManager<IdentityRole> roleManager, IConfiguration configuration, IEmailSender emailSender)
            {
                _roleManager = roleManager;
                _userManager = userManager;
                _configuration = configuration;
                _emailSender = emailSender;
            }

            public async Task<GeneralResponseDto> Handle(Request request, CancellationToken cancellationToken)
            {
                var userExists = await _userManager.FindByNameAsync(request.Model.Username);
                if (userExists != null) return GeneralResponseDto.UserAlreadyExists();

                var user = new ApplicationUser()
                {
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = request.Model.Username,
                    Email = request.Model.Email,
                    TwoFactorEnabled = true
                };

                var result = await _userManager.CreateAsync(user, request.Model.Password);
                if (!result.Succeeded) return GeneralResponseDto.UserCreationFailure();

                if (await _roleManager.RoleExistsAsync(UserRolesConstants.Admin))
                    await _userManager.AddToRoleAsync(user, UserRolesConstants.Admin);

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var returnUrl = _configuration["URL:EmailConfirmation"] + "?token=" + HttpUtility.UrlEncode(token) + "&email=" + request.Model.Email;
                _emailSender.Send(request.Model.Email, "Email confiramtion", returnUrl);

                return GeneralResponseDto.UserCreatedSuccessfully();
            }
        }
    }
}