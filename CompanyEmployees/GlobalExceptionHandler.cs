using System.Net;
using LoggingService;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CompanyEmployees;

public class GlobalExceptionHandler(ILoggerManager logger, IProblemDetailsService problemDetailsService)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        httpContext.Response.ContentType = "application/json";

        logger.LogError($"💥 Something went wrong: {exception.Message}");

        var result = await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails =
            {
                Title = "An error occurred",
                Status = httpContext.Response.StatusCode,
                Detail = "Internal Server Error",
                Type = exception.GetType().Name
            },
            Exception = exception
        });

        if (!result)
            await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Title = "An error occured",
                Status = httpContext.Response.StatusCode,
                Detail = "Internal Server Error.",
                Type = exception.GetType().Name
            }, cancellationToken);

        return true;
    }
}