using AutoMapper;
using ForumDomainEvent = TFA.Forums.Storage.Models.ForumDomainEvent;

namespace TFA.Forums.Storage.Mapping;

public class DomainEventsProfile : Profile
{
    public DomainEventsProfile()
    {
        CreateMap<Domain.DomainEvents.ForumDomainEvent, ForumDomainEvent>();
        CreateMap<Domain.DomainEvents.ForumDomainEvent.ForumComment, ForumDomainEvent.ForumComment>();
    }
}