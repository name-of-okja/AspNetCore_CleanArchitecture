using CleanArchitecture.Application.Contracts.Infrastructure;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.UnitTests.Mocks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace CleanArchitecture.Application.UnitTests.Features.Streamers;
public class CreateXUnitTest
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;

    public CreateXUnitTest()
    {
        var mapperConfig = new MapperConfiguration(c =>
        {
            c.AddProfile<MappingProfiles>();
        });
        _mapper = new Mapper(mapperConfig);

        _unitOfWork = new MockUnitOfWork(new FakeStreamerDbContext());

        _emailService = Substitute.For<IEmailService>();
    }

    [Fact]
    public async Task CreateStreamer_InputStreamer_ReturnName()
    {
        // Arrange
        var streamerInput = new Application.Features.Streamers.Create.Command(new Application.Features.Streamers.Dtos.CreateStreamerDto { Name = "TestStreamer", Url = "www.test.com" });
        var mockLogger = NullLogger<Application.Features.Streamers.Create.Handler>.Instance;
        var handler = new Application.Features.Streamers.Create.Handler(_unitOfWork, _mapper, _emailService, mockLogger);

        // Act
        var result = await handler.Handle(streamerInput, CancellationToken.None);

        // Assert
        Assert.IsType<int>(result);
        Assert.Equal(4, result);
    }
}

