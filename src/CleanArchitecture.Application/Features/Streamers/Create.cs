using AutoMapper;
using CleanArchitecture.Application.Contracts.Infrastructure;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.Features.Streamers.Dtos;
using CleanArchitecture.Application.Models.Email;
using CleanArchitecture.Domain;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.Features.Streamers;
public class Create
{
    public class Command : IRequest<int>
    {
        public CreateStreamerDto Payload { get; }
        public Command(CreateStreamerDto payload)
        {
            Payload = payload;
        }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Payload)
                .NotNull()
                .SetValidator(new CreateStreamerDtoValidator());
        }
    }

    public class CreateStreamerDtoValidator : AbstractValidator<CreateStreamerDto>
    {
        public CreateStreamerDtoValidator()
        {
            RuleFor(x => x.Name)
                    .NotEmpty().WithMessage("{Name} is required")
                    .MaximumLength(50).WithMessage("{Name} Max Length 50");
            RuleFor(x => x.Url).NotEmpty()
                    .NotEmpty().WithMessage("{Url} is required");
        }
    }

    public class Handler : IRequestHandler<Command, int>
    {
        // private readonly IStreamerRepository _streamerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly ILogger<Handler> _logger;

        public Handler(IUnitOfWork unitOfWork, IMapper mapper,
                    IEmailService emailService, ILogger<Handler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _emailService = emailService;
            _logger = logger;
        }
        public async Task<int> Handle(Command request, CancellationToken cancellationToken)
        {
            var streamerEntity = _mapper.Map<Streamer>(request.Payload);

            //var streamerEntity = await _streamerRepository.AddAsync(streamer);

            _unitOfWork.StreamerRepository.AddEntity(streamerEntity);

            var result = await _unitOfWork.CompleteAsync();

            if (result <= 0)
            {
                throw new Exception($"Streamer Create Error");
            }

            _logger.LogInformation($"[Streamer] [Create]  {streamerEntity.Id}");

            await SendEmail(streamerEntity);

            return streamerEntity.Id;
        }

        private async Task SendEmail(Streamer streamer)
        {
            var email = new Email
            {
                To = "test@test.com",
                Subject = $"Create Streamer {streamer.Name}",
                Body = $"{streamer.Name} : {streamer.Url}"
            };

            try
            {
                await _emailService.SendEmail(email);
            }
            catch
            {
                _logger.LogError($"Email Send Failed Streamer Id :  {streamer.Id}");
            }
        }
    }
}
