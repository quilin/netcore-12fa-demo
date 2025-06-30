using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TFA.Forums.API.Models;
using TFA.Forums.Domain.UseCases.CreateComment;

namespace TFA.Forums.API.Controllers;

[ApiController]
[Route("topics")]
public class TopicController(
    ISender mediator,
    IMapper mapper) : ControllerBase
{
    [HttpPost("{topicId:guid}/comments")]
    [ProducesResponseType(400)]
    [ProducesResponseType(403)]
    [ProducesResponseType(200, Type = typeof(Comment))]
    public async Task<IActionResult> CreateComment(
        Guid topicId,
        [FromBody] CreateComment request,
        CancellationToken cancellationToken)
    {
        var command = new CreateCommentCommand(topicId, request.Text);
        var comment = await mediator.Send(command, cancellationToken);
        return Ok(mapper.Map<Comment>(comment));
    }
}