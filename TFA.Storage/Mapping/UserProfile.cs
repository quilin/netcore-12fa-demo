using AutoMapper;
using TFA.Domain.UseCases.SignIn;

namespace TFA.Storage.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, RecognisedUser>();
    }
}