using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Storage;

namespace CleanArchitecture.Infrastructure.Repositories;
public class UnitOfWork : IUnitOfWork
{
    private readonly StreamerDbContext _context;
    private IDbContextTransaction _transaction;
    private bool _isCommitted = false;

    private Lazy<IVideoRepository> _videoRepository;
    private Lazy<IStreamerRepository> _streamerRepository;
    private Lazy<IDirectorRepository> _directorRepository;

    public IVideoRepository VideoRepository => _videoRepository.Value;
    public IStreamerRepository StreamerRepository => _streamerRepository.Value;
    public IDirectorRepository DirectorRepository => _directorRepository.Value;

    public UnitOfWork(StreamerDbContext context
        , Lazy<IVideoRepository> videoRepository, Lazy<IStreamerRepository> streamerRepository
        , Lazy<IDirectorRepository> directorRepository)
    {
        _context = context;
        _videoRepository = videoRepository;
        _streamerRepository = streamerRepository;
        _directorRepository = directorRepository;

        _transaction = _context.Database.BeginTransaction();
    }

    public async Task<int> CompleteAsync()
    {
        if (_isCommitted)
        {
            throw new InvalidOperationException("트랜잭션이 이미 커밋되었습니다.");
        }

        try
        {
            var result = await _context.SaveChangesAsync();
            _transaction.Commit();
                _isCommitted = true;
            return result;
        }
        catch (Exception ex)
        {
            _transaction.Rollback();
            throw new Exception("Complete Error");
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }

    //public IAsyncRepository<TEntity> Repository<TEntity>() where TEntity : BaseDomainModel
    //{
    //    if (_repositories == null)
    //    {
    //        _repositories = new Hashtable();
    //    }

    //    var type = typeof(TEntity).Name;

    //    if (!_repositories.ContainsKey(type))
    //    {
    //        var repositoryType = typeof(RepositoryBase<>);
    //        var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);
    //        _repositories.Add(type, repositoryInstance);
    //    }

    //    return (IAsyncRepository<TEntity>)_repositories[type];
    //}

    //private Type GetRepsoitryType(Type entityType) => entityType switch 
    //{
    //    Type type when type == typeof(Video)  =>  typeof(VideoRepository),
    //    Type type when type == typeof(Streamer) => typeof(StreamerRepository),
    //    _ => typeof(RepositoryBase<>).MakeGenericType(entityType),
    //};
}
