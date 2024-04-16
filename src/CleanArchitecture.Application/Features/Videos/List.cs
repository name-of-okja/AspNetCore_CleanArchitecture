using AutoMapper;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.Features.Videos.Dtos;
using MediatR;

namespace CleanArchitecture.Application.Features.Videos;
public class List
{
    public class Query : IRequest<List<VideoDto>>
    {
        public string Username { get; set; } = string.Empty;
    }

    public class Handler : IRequestHandler<Query, List<VideoDto>>
    {
        private readonly IVideoRepository _videoRepository;
        private readonly IMapper _mapper;
        public Handler(IVideoRepository videoRepository, IMapper mapper)
        {
            _videoRepository = videoRepository;
            _mapper = mapper;
        }
        public async Task<List<VideoDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var videoList = await _videoRepository.GetVideosByUsername(request.Username);

            return _mapper.Map<List<VideoDto>>(videoList);
        }
    }
}
