using AutoMapper;
using TFA.Forums.Domain.Models;

namespace TFA.Forums.API.Mapping;

internal class ApiProfile : Profile
{
    public ApiProfile()
    {
        CreateMap<Forum, Models.Forum>();
        CreateMap<Topic, Models.Topic>();
    }
}