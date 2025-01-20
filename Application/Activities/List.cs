using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class List
    {
        public class Query : IRequest<Result<IEnumerable<ActivityDto>>> { }
        public class Handler : IRequestHandler<Query, Result<IEnumerable<ActivityDto>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<Result<IEnumerable<ActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var activites = await _context.Activities
                    .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();
                return Result<IEnumerable<ActivityDto>>.Success(activites);
            }
        }
    }
}