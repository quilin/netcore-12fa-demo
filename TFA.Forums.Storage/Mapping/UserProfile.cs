using AutoMapper;
using TFA.Forums.Domain.UseCases.SignIn;
using TFA.Forums.Storage.Entities;

namespace TFA.Forums.Storage.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, RecognisedUser>();
        CreateMap<Session, Forums.Domain.Authentication.Session>();
    }
}