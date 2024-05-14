using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Infrastructure.Persistence;
using CleanArchitecture.Infrastructure.Repositories;

namespace CleanArchitecture.Application.UnitTests.Mocks;
public class MockUnitOfWork : IUnitOfWork
{
    private readonly StreamerDbContext _context;

    private Lazy<IVideoRepository> _videoRepository;
    private Lazy<IStreamerRepository> _streamerRepository;
    private Lazy<IDirectorRepository> _directorRepository;

    public IStreamerRepository StreamerRepository => _streamerRepository.Value;

    public IVideoRepository VideoRepository => _videoRepository.Value;

    public IDirectorRepository DirectorRepository => _directorRepository.Value;
    public MockUnitOfWork(StreamerDbContext context)
    {
        _context = context;
        _streamerRepository = new Lazy<IStreamerRepository>(() => new StreamerRepository(_context));
        _videoRepository = new Lazy<IVideoRepository>(() => new VideoRepository(_context));
        _directorRepository = new Lazy<IDirectorRepository>(() => new DirectorRepository(_context));
    }

    public Task<int> CompleteAsync()
    {
        return _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
