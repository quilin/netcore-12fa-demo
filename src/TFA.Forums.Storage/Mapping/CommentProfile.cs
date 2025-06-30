using AutoMapper;
using TFA.Forums.Storage.Entities;

namespace TFA.Forums.Storage.Mapping;

internal class CommentProfile : Profile
{
    public CommentProfile()
    {
        CreateMap<Comment, Domain.Models.Comment>()
            .ForMember(c => c.Id, s => s.MapFrom(c => c.CommentId));
    }
}