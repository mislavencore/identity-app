using API.Controllers.Base;
using Application.Authentication.Commands;
using Application.Authentication.Dto;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers.Authentication
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : BaseController
    {
        public AuthenticationController(IMediator mediator) : base(mediator) { }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModelDto model)
            => Ok(await Mediator.Send(new LoginUserCommand.Request(model)));

        [HttpPost("LoginVerification")]
        public async Task<IActionResult> LoginVerification([FromBody] TwoStepAuthenticationDto model)
            => Ok(await Mediator.Send(new LoginVerificationCommand.Request(model)));

        [HttpGet("EmailVerification")]
        public async Task<IActionResult> EmailVerification(string email, string token)
            => Ok(await Mediator.Send(new EmailVerificationCommand.Request(email, token)));

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterModelDto model)
            => Ok(await Mediator.Send(new RegisterUserCommand.Request(model)));

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken(TokenModelDto tokenModel)
            => Ok(await Mediator.Send(new RefreshTokenCommand.Request(tokenModel)));

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto forgottenPassword)
            => Ok(await Mediator.Send(new ForgotPasswordCommand.Request(forgottenPassword)));

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetData)
            => Ok(await Mediator.Send(new ResetPasswordCommand.Request(resetData)));

        [Authorize(Roles = "Admin")]
        [HttpPost("RegisterAdmin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModelDto model)
            => Ok(await Mediator.Send(new RegisterAdminCommand.Request(model)));

        [Authorize(Roles = "Admin")]
        [HttpPost("PermanentUserLock")]
        public async Task<IActionResult> PermanentUserLock([FromBody] string userId)
            => Ok(await Mediator.Send(new PermanentUserLockCommand.Request(userId)));

        [Authorize]
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout(string username) 
            => Ok(await Mediator.Send(new LogoutUserCommand.Request(username)));
    }
}