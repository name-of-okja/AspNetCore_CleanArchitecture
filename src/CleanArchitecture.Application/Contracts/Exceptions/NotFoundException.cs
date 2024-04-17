namespace CleanArchitecture.Application.Contracts.Exceptions;
public class NotFoundException : ApplicationException
{
    public NotFoundException(string name, object key)
        : base($"Entity \"${name}\" ({key}) Not Found  ") { }
}
