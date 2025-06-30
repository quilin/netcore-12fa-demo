using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using TFA.Search.API.Grpc;
using TFA.Search.ForumConsumer.Monitoring;

namespace TFA.Search.ForumConsumer;

internal class ForumSearchConsumer(
    IConsumer<byte[], byte[]> consumer,
    SearchEngine.SearchEngineClient searchEngineClient,
    IOptions<ConsumerConfig> consumerConfig) : BackgroundService
{
    private readonly ConsumerConfig consumerConfig = consumerConfig.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();

        consumer.Subscribe("tfa.DomainEvents");

        while (!stoppingToken.IsCancellationRequested)
        {
            var consumeResult = consumer.Consume(stoppingToken);
            if (consumeResult is not { IsPartitionEOF: false })
            {
                await Task.Delay(300, stoppingToken);
                continue;
            }

            var activityId = consumeResult.Message.Headers.TryGetLastBytes("activity_id", out var lastBytes)
                ? Encoding.UTF8.GetString(lastBytes)
                : null;
        
            using var activity = Metrics.ActivitySource.StartActivity("consumer", ActivityKind.Consumer,
                ActivityContext.TryParse(activityId, null, out var context) ? context : default);
            activity?.AddTag("messaging.system", "kafka");
            activity?.AddTag("messaging.destination.name", "tfa.DomainEvents");
            activity?.AddTag("messaging.kafka.consumer_group", consumerConfig.GroupId);
            activity?.AddTag("messaging.kafka.partition", consumeResult.Partition);
        
            var domainEventWrapper = JsonSerializer.Deserialize<DomainEventWrapper>(consumeResult.Message.Value)!;
            var contentBlob = Convert.FromBase64String(domainEventWrapper.ContentBlob);
            var domainEvent = JsonSerializer.Deserialize<ForumDomainEvent>(contentBlob)!;
        
            switch (domainEvent.EventType)
            {
                case ForumDomainEventType.TopicCreated:
                    await searchEngineClient.IndexAsync(new IndexRequest
                    {
                        Id = domainEvent.TopicId.ToString(),
                        Type = SearchEntityType.ForumTopic,
                        Title = domainEvent.Title
                    }, cancellationToken: stoppingToken);
                    break;
                case ForumDomainEventType.CommentCreated:
                    await searchEngineClient.IndexAsync(new IndexRequest
                    {
                        Id = domainEvent.Comment!.CommentId.ToString(),
                        Type = SearchEntityType.ForumComment,
                        Text = domainEvent.Comment.Text
                    }, cancellationToken: stoppingToken);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            consumer.Commit(consumeResult);
        }
        consumer.Close();
    }
}