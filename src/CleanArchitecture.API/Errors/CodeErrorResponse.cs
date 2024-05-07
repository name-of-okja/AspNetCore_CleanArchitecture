using System.Net;

namespace CleanArchitecture.API.Errors;

public class CodeErrorResponse
{
    public required int StatusCode { get; set; }
    private string _message;
    public string Message
    {
        get => _message ?? GetDefaultMessageStatusCode(StatusCode);
        set => _message = value;
    }

    private string GetDefaultMessageStatusCode(int statusCode) => (HttpStatusCode)statusCode switch
    {
        HttpStatusCode.BadRequest => "Bad Request",
        HttpStatusCode.Unauthorized => "Unauthorized",
        HttpStatusCode.NotFound => "Not Found",
        HttpStatusCode.InternalServerError => "ServerError",
        _ => "Unknown Error"
    };
}
