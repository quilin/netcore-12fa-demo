using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TFA.Storage;

public class Topic
{
    [Key]
    public Guid TopicId { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    public Guid UserId { get; set; }

    public Guid ForumId { get; set; }

    public string Title { get; set; }

    [ForeignKey(nameof(UserId))]
    public User Author { get; set; }

    [ForeignKey(nameof(ForumId))]
    public Forum Forum { get; set; }

    [InverseProperty(nameof(Comment.Topic))]
    public ICollection<Comment> Comments { get; set; }
}