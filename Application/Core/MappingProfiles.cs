using Application.Activities;
using Application.Comments;
using AutoMapper;
using Domain;

namespace Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() {
            string currentUsername = null;
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
                .ForMember(o => o.Image, x => x.MapFrom(a => a.AppUser.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(o => o.FollowersCount, x => x.MapFrom(a => a.AppUser.Followers.Count))
                .ForMember(o => o.FollowingCount, x => x.MapFrom(a => a.AppUser.Followings.Count))
                .ForMember(o => o.Following, x => x.MapFrom(a => a.AppUser.Followers.Any(c => c.Observer.UserName == currentUsername)));
            CreateMap<AppUser, Profiles.Profile>()
                .ForMember(d => d.Image, o => o.MapFrom(x => x.Photos.FirstOrDefault(a => a.IsMain).Url))
                .ForMember(d => d.FollowersCount, o => o.MapFrom(x => x.Followers.Count))
                .ForMember(d => d.FollowingCount, o => o.MapFrom(x => x.Followings.Count))
                .ForMember(d => d.Following, o => o.MapFrom(x => x.Followers.Any(a => a.Observer.UserName == currentUsername)));
            CreateMap<Comment, CommentDto>()
                .ForMember(o => o.DisplayName, x => x.MapFrom(t => t.Author.DisplayName))
                .ForMember(o => o.UserName, x => x.MapFrom(t => t.Author.UserName))
                .ForMember(o => o.Image, x => x.MapFrom(t => t.Author.Photos.FirstOrDefault(x => x.IsMain).Url));
            CreateMap<ActivityAttendee, Profiles.UserActivityDto>()
                .ForMember(o => o.Category, x => x.MapFrom(t => t.Activity.Category))
                .ForMember(o => o.Title, x => x.MapFrom(t => t.Activity.Title))
                .ForMember(o => o.Id, x => x.MapFrom(t => t.Activity.Id))
                .ForMember(o => o.HostUsername, x => x.MapFrom(t => t.Activity.Attendees.FirstOrDefault(u => u.IsHost).AppUser.UserName))
                .ForMember(o => o.Date, x => x.MapFrom(t => t.Activity.Date));
        }
    }
}