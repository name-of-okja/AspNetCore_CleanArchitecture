using CleanArchitecture.Application.Contracts.Identity;

namespace CleanArchitecture.API.IntegrationTest.Utills;
public class UserAccessorServiceForTest : IUserAccessorService
{
    public string GetUserEamil()
    {
        return "test@test.com";
    }

    public string GetUserId()
    {
        return "testUserID";
    }

    public string GetUserName()
    {
        return "testUserName";
    }
}
