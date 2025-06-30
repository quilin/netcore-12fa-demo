using System.ComponentModel.DataAnnotations;

namespace TFA.Forums.Storage.Entities;

public class DomainEvent
{
    [Key]
    public Guid DomainEventId { get; set; }
    
    public DateTimeOffset EmittedAt { get; set; }

    [MaxLength(55)]
    public string? ActivityId { get; set; }

    [Required]
    public required byte[] ContentBlob { get; set; }
}