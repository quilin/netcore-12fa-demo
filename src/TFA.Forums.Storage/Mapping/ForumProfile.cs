using AutoMapper;
using TFA.Forums.Domain.Models;

namespace TFA.Forums.Storage.Mapping;

internal class ForumProfile : Profile
{
    public ForumProfile()
    {
        CreateMap<Entities.Forum, Forum>()
            .ForMember(d => d.Id, s => s.MapFrom(f => f.ForumId));
    }
}