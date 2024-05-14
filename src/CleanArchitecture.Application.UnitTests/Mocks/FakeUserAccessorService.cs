using CleanArchitecture.Application.Contracts.Identity;

namespace CleanArchitecture.Application.UnitTests.Mocks;
public static class FakeUserAccessorService
{
    public static IUserAccessorService GetUserAccessorService()
    {
        var fakeUserAccessorService = Substitute.For<IUserAccessorService>();
        fakeUserAccessorService.GetUserEamil().Returns("test@test.com");
        fakeUserAccessorService.GetUserName().Returns("test");

        return fakeUserAccessorService;
    }
}
