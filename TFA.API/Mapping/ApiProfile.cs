using AutoMapper;
using TFA.Domain.Models;

namespace TFA.API.Mapping;

internal class ApiProfile : Profile
{
    public ApiProfile()
    {
        CreateMap<Forum, Models.Forum>();
        CreateMap<Topic, Models.Topic>();
    }
}