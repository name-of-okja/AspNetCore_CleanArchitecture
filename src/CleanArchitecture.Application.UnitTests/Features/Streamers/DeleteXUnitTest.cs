using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.UnitTests.Mocks;
using Microsoft.Extensions.Logging.Abstractions;

namespace CleanArchitecture.Application.UnitTests.Features.Streamers;
public class DeleteXUnitTest
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteXUnitTest()
    {
        var mapperConfig = new MapperConfiguration(c =>
        {
            c.AddProfile<MappingProfiles>();
        });
        _mapper = new Mapper(mapperConfig);

        _unitOfWork = new MockUnitOfWork(new FakeStreamerDbContext());
    }

    [Fact]
    public async Task DeleteStreamer_InputStreamerById_ReturnsUnit()
    {
        // Arrange
        var streamer = new Application.Features.Streamers.Delete.Command(3);
        var mockLogger = NullLogger<Application.Features.Streamers.Delete.Handler>.Instance;
        var handler = new Application.Features.Streamers.Delete.Handler(_unitOfWork.StreamerRepository, _mapper, mockLogger);

        // Act
        var result = await handler.Handle(streamer, CancellationToken.None);

        // Assert
        Assert.IsType<MediatR.Unit>(result);
    }
}
