using MediatR;
using Microsoft.AspNetCore.Mvc;
using TFA.Forums.API.Authentication;
using TFA.Forums.API.Models;
using TFA.Forums.Domain.UseCases.SignIn;
using TFA.Forums.Domain.UseCases.SignOn;

namespace TFA.Forums.API.Controllers;

[ApiController]
[Route("account")]
public class AccountController(ISender mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> SignOn(
        [FromBody] SignOn request,
        CancellationToken cancellationToken)
    {
        var identity = await mediator.Send(new SignOnCommand(request.Login, request.Password), cancellationToken);
        return Ok(identity);
    }

    [HttpPost("signin")]
    public async Task<IActionResult> SignIn(
        [FromBody] SignIn request,
        [FromServices] IAuthTokenStorage tokenStorage,
        CancellationToken cancellationToken)
    {
        var (identity, token) = await mediator.Send(
            new SignInCommand(request.Login, request.Password), cancellationToken);
        tokenStorage.Store(HttpContext, token);
        return Ok(identity);
    }
}