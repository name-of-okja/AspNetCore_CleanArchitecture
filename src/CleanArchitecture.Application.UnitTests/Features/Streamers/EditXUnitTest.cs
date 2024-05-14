using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.UnitTests.Mocks;
using Microsoft.Extensions.Logging.Abstractions;

namespace CleanArchitecture.Application.UnitTests.Features.Streamers;
public class EditXUnitTest
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public EditXUnitTest()
    {
        var mapperConfig = new MapperConfiguration(c =>
        {
            c.AddProfile<MappingProfiles>();
        });
        _mapper = new Mapper(mapperConfig);

        _unitOfWork = new MockUnitOfWork(new FakeStreamerDbContext());
    }

    [Fact]
    public async Task EditStreamer_InputStreamer_ReturnsUnit()
    {
        // Arrange
        var updateStreamer = new Application.Features.Streamers.Edit.Command(1)
        {
            Payload = new Application.Features.Streamers.Dtos.EditStreamerDto()
            {
                Name = "EditTest",
            }
        };
        var mockLogger = NullLogger<Application.Features.Streamers.Edit.Handler>.Instance;
        var updateStreamerHandler = new Application.Features.Streamers.Edit.Handler(_unitOfWork, _mapper, mockLogger);

        // Act
        var result = await updateStreamerHandler.Handle(updateStreamer, CancellationToken.None);

        // Assert
        Assert.IsType<MediatR.Unit>(result);
        var updatedStreamer = await _unitOfWork.StreamerRepository.GetByIdAsync(1);
        Assert.Equal(updateStreamer.Payload.Name, updatedStreamer.Name);
    }
}
