using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Photos
{
    public class Add
    {
        public class Command : IRequest<Result<Photo>>
        {
            public IFormFile File { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Photo>>
        {
            private readonly IPhotoAccessor _photoAccessor;
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;
            public Handler(DataContext context, IPhotoAccessor photoAccessor, IUserAccessor userAccessor)
            {
                _context = context;
                _photoAccessor = photoAccessor;
                _userAccessor = userAccessor;

            }

            public async Task<Result<Photo>> Handle(Command request, CancellationToken cancellationToken)
            {
                var userName = _userAccessor.GetUserName();
                var user = await _context.Users.Include(x => x.Photos)
                    .FirstOrDefaultAsync(x => x.UserName == userName);
                
                if(user == null) return null;

                var photoUploadResult = await _photoAccessor.AddPhoto(request.File);
                if(photoUploadResult == null) return null;
                
                var photo = new Photo {
                    Id = photoUploadResult.PublicId,
                    Url = photoUploadResult.Url
                };

                if(!user.Photos.Any(x => x.IsMain)) photo.IsMain = true;

                user.Photos.Add(photo);
                var result = await _context.SaveChangesAsync() > 0;

                return result ? Result<Photo>.Success(photo) : Result<Photo>.Failure("Problem adding photo");
            }
        }
    }
}