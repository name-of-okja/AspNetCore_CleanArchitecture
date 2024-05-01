
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Domain;
using CleanArchitecture.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Repositories;
public class VideoRepository : RepositoryBase<Video>, IVideoRepository
{
    public VideoRepository(StreamerDbContext context) : base(context)
    {
        
    }
    public async Task<Video> GetVideoByName(string videoName)
    {
        return await _context.Videos.Where(o => o.Name == videoName).SingleOrDefaultAsync();
    }

    public async Task<IEnumerable<Video>> GetVideosByUsername(string username)
    {
        return await _context.Videos.Where(v => v.CreatedBy == username).ToListAsync();
    }
}
