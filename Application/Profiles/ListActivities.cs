using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles
{
    public class ListActivities
    {
        public class Query : IRequest<Result<List<UserActivityDto>>>
        {
            public string Predicate { get; set; }
            public string Username { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<UserActivityDto>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<List<UserActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == request.Username);
                if(user == null) return null;
                var activites = await _context.ActivityAttendees
                    .Where(x => x.AppUser.UserName == request.Username)
                    .ProjectTo<UserActivityDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                switch(request.Predicate) {
                    case "past":
                        activites = activites
                                .Where(x => x.Date < DateTime.UtcNow)
                                .ToList();
                        break;
                    case "future":
                        activites = activites
                                .Where(x => x.Date >= DateTime.UtcNow)
                                .ToList();
                        break;
                    default:
                        activites = activites.Where(x => x.HostUsername == request.Username).ToList();
                        break;
                }

                return Result<List<UserActivityDto>>.Success(activites);
            }
        }
    }
}