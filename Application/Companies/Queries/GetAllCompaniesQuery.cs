using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistance;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Companies.Queries
{
    public class GetAllCompaniesQuery
    {
        public class Request : IRequest<object> { }

        public class Handler : IRequestHandler<Request, object>
        {
            private readonly AppDbContext _dbContext;

            public Handler(AppDbContext dbContext) 
                => _dbContext = dbContext;

            public async Task<object> Handle(Request request, CancellationToken cancellationToken) 
                => new { Data = await _dbContext.Set<Company>().ToListAsync() };
        }
    }
}