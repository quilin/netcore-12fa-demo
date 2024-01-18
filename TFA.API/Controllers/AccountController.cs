using MediatR;
using Microsoft.AspNetCore.Mvc;
using TFA.API.Authentication;
using TFA.API.Models;
using TFA.Domain.UseCases.SignIn;
using TFA.Domain.UseCases.SignOn;

namespace TFA.API.Controllers;

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