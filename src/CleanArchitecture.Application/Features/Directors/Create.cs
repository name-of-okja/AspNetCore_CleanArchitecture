using AutoMapper;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.Features.Directors.Dtos;
using CleanArchitecture.Domain;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.Features.Directors;
public class Create
{
    public class Command : IRequest<int>
    {
        public CreateDirectorDto Payload { get; }
        public Command(CreateDirectorDto payload)
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
                .SetValidator(new CreateDirectorDtoValidator());
        }
    }

    public class CreateDirectorDtoValidator : AbstractValidator<CreateDirectorDto>
    {
        public CreateDirectorDtoValidator()
        {
            RuleFor(x => x.Name)
                    .NotEmpty().WithMessage("{Name} is required")
                    .MaximumLength(50).WithMessage("{Name} Max Length 50");
            RuleFor(x => x.LastName).NotEmpty()
                    .NotEmpty().WithMessage("{LastName} is required");
        }
    }
    public class Handler : IRequestHandler<Command, int>
    {
        private readonly ILogger<Handler> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public Handler(ILogger<Handler> logger, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(Command request, CancellationToken cancellationToken)
        {
            var directorEntity = _mapper.Map<Director>(request.Payload);

            _unitOfWork.DirectorRepository.AddEntity(directorEntity);

            var result = await _unitOfWork.CompleteAsync();

            if(result <= 0)
            {
                _logger.LogError("No Insert Directory");
                throw new Exception("No Insert Entity");
            }

            return directorEntity.Id;
        }
    }
}
