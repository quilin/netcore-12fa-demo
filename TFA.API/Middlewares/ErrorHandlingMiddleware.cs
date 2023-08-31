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
        ILogger<ErrorHandlingMiddleware> logger,
        ProblemDetailsFactory problemDetailsFactory)
    {
        try
        {
            logger.LogError("Error handling started for request in path {RequestPath}", httpContext.Request.Path.Value);
            await next.Invoke(httpContext);
        }
        catch (Exception exception)
        {
            logger.LogError(
                exception,
                "Error has happened with {RequestPath}, the message is {ErrorMessage}",
                httpContext.Request.Path.Value, exception.Message);

            ProblemDetails problemDetails;
            switch (exception)
            {
                case IntentionManagerException intentionManagerException:
                    problemDetails = problemDetailsFactory.CreateFrom(httpContext, intentionManagerException);
                    break;
                case ValidationException validationException:
                    problemDetails = problemDetailsFactory.CreateFrom(httpContext, validationException);
                    logger.LogInformation(validationException, "Somebody sent invalid request, oops");
                    break;
                case DomainException domainException:
                    problemDetails = problemDetailsFactory.CreateFrom(httpContext, domainException);
                    logger.LogError(domainException, "Domain exception occured");
                    break;
                default:
                    problemDetails = problemDetailsFactory.CreateProblemDetails(
                        httpContext, StatusCodes.Status500InternalServerError, "Unhandled error! Please contact us.");
                    logger.LogError(exception, "Unhandled exception occured");
                    break;
            }

            httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, problemDetails.GetType());
        }
    }
}