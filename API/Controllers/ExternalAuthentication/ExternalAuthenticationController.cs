using API.Controllers.Base;
using Application.Authentication.Dto;
using Application.ExternalAuthentication.Commands;
using Application.ExternalAuthentication.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers.ExternalAuthentication
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExternalAuthenticationController : BaseController
    {
        public ExternalAuthenticationController(IMediator mediator) : base(mediator) { }

        [HttpGet("Google")]
        public async Task<TokenModelStatusDto> Google(string tokenId)
        {
            var payload = await Mediator.Send(new GoogleVerificationQuery.Request(tokenId));
            if (payload == null) return TokenModelStatusDto.Unauthorized();

            var user = await Mediator.Send(new LoginExternalUserCommand.Request("Google", payload));
            return await Mediator.Send(new GenerateExternalUserTokenCommand.Request(user));
        }

        [HttpGet("Facebook")]
        public async Task<TokenModelStatusDto> Facebook(string accessToken)
        {
            var payload = await Mediator.Send(new FacebookVerificationQuery.Request(accessToken));
            if (payload == null) return TokenModelStatusDto.Unauthorized();

            var user = await Mediator.Send(new LoginExternalUserCommand.Request("Facebook", payload));
            return await Mediator.Send(new GenerateExternalUserTokenCommand.Request(user));
        }
    }
}