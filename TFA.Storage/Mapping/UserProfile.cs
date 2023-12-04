using AutoMapper;
using TFA.Domain.UseCases.SignIn;
using TFA.Storage.Entities;

namespace TFA.Storage.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, RecognisedUser>();
        CreateMap<Session, Domain.Authentication.Session>();
    }
}