using API.Controllers.Base;
using Application.Companies.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers.Company
{
    [Authorize]
    public class CompanyController : BaseController
    {
        public CompanyController(IMediator mediator) : base(mediator) { }

        [HttpPost(nameof(GetAll))]
        public async Task<IActionResult> GetAll() 
            => Ok(await Mediator.Send(new GetAllCompaniesQuery.Request()));
    }
}