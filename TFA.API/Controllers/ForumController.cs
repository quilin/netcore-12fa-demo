using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TFA.API.Models;
using TFA.Domain.UseCases.CreateForum;
using TFA.Domain.UseCases.CreateTopic;
using TFA.Domain.UseCases.GetForums;
using TFA.Domain.UseCases.GetTopics;
using Forum = TFA.API.Models.Forum;
using Topic = TFA.API.Models.Topic;

namespace TFA.API.Controllers;

[ApiController]
[Route("forums")]
public class ForumController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(400)]
    [ProducesResponseType(403)]
    [ProducesResponseType(201, Type = typeof(Forum))]
    public async Task<IActionResult> CreateForum(
        [FromBody] CreateForum request,
        [FromServices] IMapper mapper,
        CancellationToken cancellationToken)
    {
        var command = new CreateForumCommand(request.Title);
        var forum = await mediator.Send(command, cancellationToken);
        return CreatedAtRoute(nameof(GetForums), mapper.Map<Forum>(forum));
    }

    /// <summary>
    /// Get list of every forum
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet(Name = nameof(GetForums))]
    [ProducesResponseType(200, Type = typeof(Forum[]))]
    public async Task<IActionResult> GetForums(
        [FromServices] IMapper mapper,
        CancellationToken cancellationToken)
    {
        var forums = await mediator.Send(new GetForumsQuery(), cancellationToken);
        return Ok(forums.Select(mapper.Map<Forum>));
    }

    [HttpPost("{forumId:guid}/topics")]
    [ProducesResponseType(400)]
    [ProducesResponseType(403)]
    [ProducesResponseType(410)]
    [ProducesResponseType(201, Type = typeof(Topic))]
    public async Task<IActionResult> CreateTopic(
        Guid forumId,
        [FromBody] CreateTopic request,
        [FromServices] IMapper mapper,
        CancellationToken cancellationToken)
    {
        var command = new CreateTopicCommand(forumId, request.Title);
        var topic = await mediator.Send(command, cancellationToken);
        return CreatedAtRoute(nameof(GetForums), mapper.Map<Topic>(topic));
    }

    [HttpGet("{forumId:guid}/topics")]
    [ProducesResponseType(400)]
    [ProducesResponseType(410)]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetTopics(
        [FromRoute] Guid forumId,
        [FromQuery] int skip,
        [FromQuery] int take,
        [FromServices] IMapper mapper,
        CancellationToken cancellationToken)
    {
        var query = new GetTopicsQuery(forumId, skip, take);
        var (resources, totalCount) = await mediator.Send(query, cancellationToken);
        return Ok(new { resources = resources.Select(mapper.Map<Topic>), totalCount });
    }
}