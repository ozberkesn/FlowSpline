using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;

namespace FlowSpline.Api.Middleware;

internal sealed class ValidationExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken ct)
    {
        if (exception is not ValidationException ve)
            return false;

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        var errors = ve.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

        await httpContext.Response.WriteAsJsonAsync(new { errors }, ct);
        return true;
    }
}
