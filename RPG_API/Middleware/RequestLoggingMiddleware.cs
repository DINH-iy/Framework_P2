using System.Diagnostics;
using Game.Domain;
using Game.Domain.Entities;

namespace Game.Api.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext, GameDbContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        string? exceptionMessage = null;

        try
        {
            await _next(httpContext);
        }
        catch (Exception exception)
        {
            exceptionMessage = exception.Message;
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            throw;
        }
        finally
        {
            stopwatch.Stop();

            var requestLog = new ApiRequestLog
            {
                Method = httpContext.Request.Method,
                Path = httpContext.Request.Path,
                StatusCode = httpContext.Response.StatusCode,
                DurationMilliseconds = stopwatch.ElapsedMilliseconds,
                ExceptionMessage = exceptionMessage
            };

            context.ApiRequestLogs.Add(requestLog);
            await context.SaveChangesAsync();
        }
    }
}
