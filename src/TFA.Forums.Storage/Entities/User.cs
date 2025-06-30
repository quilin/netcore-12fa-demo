using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TFA.Forums.Storage.Entities;

public class User
{
    [Key]
    public Guid UserId { get; set; }

    [MaxLength(20)]
    public required string Login { get; set; }

    [MaxLength(100)]
    public required byte[] Salt { get; set; }

    [MaxLength(32)]
    public required byte[] PasswordHash { get; set; }

    [InverseProperty(nameof(Topic.Author))]
    public ICollection<Topic> Topics { get; set; } = null!;

    [InverseProperty(nameof(Comment.Author))]
    public ICollection<Comment> Comments { get; set; } = null!;

    [InverseProperty(nameof(Session.User))]
    public ICollection<Session> Sessions { get; set; } = null!;
}