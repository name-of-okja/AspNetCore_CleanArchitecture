using AutoMapper;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.Exceptions;
using CleanArchitecture.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.Features.Streamers;
public class Delete
{
    public class Command : IRequest<Unit>
    {
        public int Id { get; }
        public Command(int id)
        {
            Id = id;
        }
    }

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IStreamerRepository _streamerRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<Handler> _logger;

        public Handler(IStreamerRepository streamerRepository, IMapper mapper, ILogger<Handler> logger)
        {
            _streamerRepository = streamerRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var streamer = await _streamerRepository.GetByIdAsync(request.Id);
            if (streamer == null)
            {
                _logger.LogError($"[Streamer] [Delete] NotFound {request.Id}");
                throw new NotFoundException(nameof(Streamer), request.Id);
            }

            await _streamerRepository.DeleteAsync(streamer);

            _logger.LogInformation($"[Streamer] [Delete] {request.Id}");

            return Unit.Value;
        }
    }
}
