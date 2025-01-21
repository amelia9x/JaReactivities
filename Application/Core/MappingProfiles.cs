using Application.Activities;
using AutoMapper;
using Domain;

namespace Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() {
            CreateMap<Activity, Activity>();
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
        }
    }
}