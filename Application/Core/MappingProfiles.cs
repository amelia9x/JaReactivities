using Application.Activities;
using Application.Comments;
using AutoMapper;
using Domain;

namespace Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() {
            CreateMap<Activity, Activity>();
            // CreateMap<Profiles.Profile, AppUser>()
            //     .ForMember(x => x.UserName, a => a.Ignore());
            CreateMap<Activity, ActivityDto>()
                .ForMember(o => o.HostUserName, 
                    t => t.MapFrom(x => x.Attendees.FirstOrDefault(a => a.IsHost).AppUser.UserName));
            CreateMap<ActivityAttendee, AttendeeDto>()
                .ForMember(o => o.UserName, x => x.MapFrom(a => a.AppUser.UserName))
                .ForMember(o => o.DisplayName, x => x.MapFrom(a => a.AppUser.DisplayName))
                .ForMember(o => o.Bio, x => x.MapFrom(a => a.AppUser.Bio))
                .ForMember(o => o.Image, x => x.MapFrom(a => a.AppUser.Photos.FirstOrDefault(x => x.IsMain).Url));
            CreateMap<AppUser, Profiles.Profile>()
                .ForMember(d => d.Image, o => o.MapFrom(x => x.Photos.FirstOrDefault(a => a.IsMain).Url));
            CreateMap<Comment, CommentDto>()
                .ForMember(o => o.DisplayName, x => x.MapFrom(t => t.Author.DisplayName))
                .ForMember(o => o.UserName, x => x.MapFrom(t => t.Author.UserName))
                .ForMember(o => o.Image, x => x.MapFrom(t => t.Author.Photos.FirstOrDefault(x => x.IsMain).Url));
        }
    }
}