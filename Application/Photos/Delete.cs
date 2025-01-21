using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Photos
{
    public class Delete
    {
        public class Command : IRequest<Result<Unit>>
        {
            public string Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;
            private readonly IPhotoAccessor _photoAccessor;
            public Handler(DataContext context, IPhotoAccessor photoAccessor, IUserAccessor userAccessor)
            {
                _photoAccessor = photoAccessor;
                _userAccessor = userAccessor;
                _context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                // Find user
                var user = await _context.Users.Include(p => p.Photos)
                    .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUserName());
                if(user == null) return null;

                // Find photo
                var photo = user.Photos.FirstOrDefault(x => x.Id == request.Id);
                if(photo == null) return null;

                if(photo.IsMain) return Result<Unit>.Failure("Cannot delete the main picture");
                // Delete photo
                var result = _photoAccessor.DeletePhoto(request.Id);
                if(result == null) return Result<Unit>.Failure("Problem deleting photo from Cloudinary");

                // Remove photo from a user
                user.Photos.Remove(photo);

                // Save changes
                var success = await _context.SaveChangesAsync() > 0;

                return success ? Result<Unit>.Success(Unit.Value) : Result<Unit>.Failure("Problem deleting photo from API");
            }
        }
    }
}