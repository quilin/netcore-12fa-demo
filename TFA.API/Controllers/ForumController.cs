using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TFA.Storage;

namespace TFA.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ForumController : ControllerBase
{
    /// <summary>
    /// Get list of every forum
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(string[]))]
    public async Task<IActionResult> GetForums(
        [FromServices] ForumDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var forumTitles = await dbContext.Forums.Select(f => f.Title).ToArrayAsync(cancellationToken);
        return Ok(forumTitles);
    }
}