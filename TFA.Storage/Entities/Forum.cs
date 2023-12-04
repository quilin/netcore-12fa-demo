using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TFA.Storage.Entities;

public class Forum
{
    [Key]
    public Guid ForumId { get; set; }

    [MaxLength(50)]
    public string Title { get; set; }

    [InverseProperty(nameof(Topic.Forum))]
    public ICollection<Topic> Topics { get; set; }
}