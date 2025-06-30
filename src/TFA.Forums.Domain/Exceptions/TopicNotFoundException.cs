namespace TFA.Forums.Domain.Exceptions;

public class TopicNotFoundException(Guid topicId)
    : DomainException(DomainErrorCode.Gone, $"Topic with id {topicId} was not found");