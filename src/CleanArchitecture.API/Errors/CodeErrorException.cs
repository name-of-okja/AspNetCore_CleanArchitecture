namespace CleanArchitecture.API.Errors;

public class CodeErrorException : CodeErrorResponse
{
    public string Details { get; set; } = string.Empty;
}
