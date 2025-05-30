using AutoMapper;

namespace CoreApp.API.Features.Users;

public class MappingProfile : Profile
{
    public MappingProfile() => CreateMap<Domain.Person, User>(MemberList.None);
}
