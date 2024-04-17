using AutoMapper;
using CleanArchitecture.Application.Contracts.Exceptions;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.Features.Streamers.Dtos;
using CleanArchitecture.Domain;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.Features.Streamers;
public class Edit
{
    public class Command : IRequest<Unit>
    {     
        public int Id { get; set; }
        public EditStreamerDto payload { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.payload).SetValidator(new EditStreadmerDtoValidator());
        }
    }

    public class EditStreadmerDtoValidator : AbstractValidator<EditStreamerDto>
    {
        public EditStreadmerDtoValidator()
        {
            RuleFor(x => x.Name)
                    .MaximumLength(50).WithMessage("{Name} Max Length 50").When(x => !string.IsNullOrEmpty(x.Name));
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

            if (streamer is null)
            {
                _logger.LogError($"[Streamer] [Edit] NotFound {request.Id}");
                throw new NotFoundException(nameof(Streamer), request.Id);
            }

            // payload -> streamer update
            _mapper.Map(request.payload, streamer, typeof(EditStreamerDto), typeof(Streamer));

            await _streamerRepository.UpdateAsync(streamer);

            _logger.LogInformation($"[Streamer] [Edit] {request.Id}");

            return Unit.Value;
        }
    }
}
