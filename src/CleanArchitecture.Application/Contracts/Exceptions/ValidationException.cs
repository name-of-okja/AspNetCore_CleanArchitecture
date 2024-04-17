using FluentValidation.Results;

namespace CleanArchitecture.Application.Contracts.Exceptions;
public class ValidationException : ApplicationException
{
    public IDictionary<string, string[]> Errors { get; }
    public ValidationException() : base("Validation Errors")
    {
        Errors = new Dictionary<string, string[]>();
    }
    public ValidationException(IEnumerable<ValidationFailure> failures) : this()
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(g => g.Key, g => g.ToArray());
    }
}
