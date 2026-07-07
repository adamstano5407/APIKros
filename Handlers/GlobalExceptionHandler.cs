using APIKros.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;

namespace APIKros.Handlers;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken)
    {
        context.Response.ContentType = "application/json";

        switch (exception)
        {
            case ValidationException validationException:
                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                await context.Response.WriteAsJsonAsync(new
                {
                    type = "validation_error",
                    errors = validationException.Errors.Select(e => new
                    {
                        field = e.PropertyName,
                        message = e.ErrorMessage,
                        code = e.ErrorCode
                    })
                }, cancellationToken);

                return true;

            case NotFoundException:
                context.Response.StatusCode = StatusCodes.Status404NotFound;

                await context.Response.WriteAsJsonAsync(new
                {
                    type = "not_found",
                    message = exception.Message
                }, cancellationToken);

                return true;

            default:
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                await context.Response.WriteAsJsonAsync(new
                {
                    type = "server_error",
                    message = "Unexpected server error."
                }, cancellationToken);

                return true;
        }
    }
}