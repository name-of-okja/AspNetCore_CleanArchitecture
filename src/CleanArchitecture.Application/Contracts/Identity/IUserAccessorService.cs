namespace CleanArchitecture.Application.Contracts.Identity;
public interface IUserAccessorService
{
    public string GetUserName();
    public string GetUserId();
    public string GetUserEamil();
}
