using AutoMapper;

namespace CoreApp.API.Endpoints.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile() => CreateMap<Domain.Person, Profile>(MemberList.None);
}
