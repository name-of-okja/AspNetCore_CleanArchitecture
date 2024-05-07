using CleanArchitecture.API.Errors;
using CleanArchitecture.Application.Exceptions;
using System.Net;
using System.Net.Mime;
using System.Text.Json;

namespace CleanArchitecture.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _environment;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            context.Response.ContentType = MediaTypeNames.Application.Json;
            var statusCode = (int)HttpStatusCode.InternalServerError;
            var errorDetailMessage = ex.StackTrace;

            switch (ex)
            {
                case NotFoundException _:
                    statusCode = (int)HttpStatusCode.NotFound;
                    break;
                case ValidationException validationException:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    errorDetailMessage = JsonSerializer.Serialize(validationException.Errors);
                    break;
                case BadRequestException _:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    break;
                
            }

            var error = new CodeErrorException 
            { 
                StatusCode = statusCode, 
                Message =  ex.Message,
                Details = errorDetailMessage
            };
            

            context.Response.StatusCode = statusCode;

            await context.Response.WriteAsJsonAsync(error, options: new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }
    }
}
