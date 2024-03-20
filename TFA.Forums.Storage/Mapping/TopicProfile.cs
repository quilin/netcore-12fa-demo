using AutoMapper;
using TFA.Forums.Domain.Models;

namespace TFA.Forums.Storage.Mapping;

internal class TopicProfile : Profile
{
    public TopicProfile()
    {
        CreateMap<Entities.Topic, Topic>()
            .ForMember(d => d.Id, s => s.MapFrom(t => t.TopicId));
    }
}