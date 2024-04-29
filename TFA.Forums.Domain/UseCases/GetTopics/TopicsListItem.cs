namespace TFA.Forums.Domain.UseCases.GetTopics;

public class TopicsListItem
{
    public Guid Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string Title { get; set; }
    public int TotalCommentsCount { get; set; }
    public TopicsListLastComment? LastComment { get; set; }
}

public class TopicsListLastComment
{
    public Guid? Id { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
}