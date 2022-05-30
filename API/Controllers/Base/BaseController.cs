using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected IMediator Mediator { get; }

        public BaseController(IMediator mediator) => this.Mediator = mediator;
    }
}