using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using TFA.Domain.Authorization;
using TFA.Domain.Exceptions;

namespace TFA.API.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(
        HttpContext httpContext,
        ProblemDetailsFactory problemDetailsFactory)
    {
        try
        {
            await next.Invoke(httpContext);
        }
        catch (Exception exception)
        {
            var problemDetails = exception switch
            {
                IntentionManagerException intentionManagerException =>
                    problemDetailsFactory.CreateFrom(httpContext, intentionManagerException),
                ValidationException validationException =>
                    problemDetailsFactory.CreateFrom(httpContext, validationException),
                DomainException domainException =>
                    problemDetailsFactory.CreateFrom(httpContext, domainException),
                _ => problemDetailsFactory.CreateProblemDetails(httpContext, StatusCodes.Status500InternalServerError,
                    "Unhandled error! Please contact us.", detail: exception.Message),
            };

            httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
            if (problemDetails is ValidationProblemDetails validationProblemDetails)
            {
                await httpContext.Response.WriteAsJsonAsync(validationProblemDetails);
            }
            else
            {
                await httpContext.Response.WriteAsJsonAsync(problemDetails);
            }
        }
    }
}