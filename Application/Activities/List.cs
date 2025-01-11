using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class List
    {
        public class Query : IRequest<IEnumerable<Activity>> {}
        public class Handler : IRequestHandler<Query, IEnumerable<Activity>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<Activity>> Handle(Query request, CancellationToken cancellationToken)
            {
                var activites = await _context.Activities.ToListAsync();
                return activites;
            }
        }
    }
}