using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class List
    {
        public class Query : IRequest<Result<IEnumerable<Activity>>> {}
        public class Handler : IRequestHandler<Query, Result<IEnumerable<Activity>>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<IEnumerable<Activity>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var activites = await _context.Activities.ToListAsync();
                return Result<IEnumerable<Activity>>.Success(activites);
            }
        }
    }
}