﻿using AutoMapper;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.Exceptions;
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
        public int Id { get; }
        public EditStreamerDto Payload { get; set; }

        public Command(int id)
        {
            Id = id;
        }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Payload)
                .NotNull()
                .SetValidator(new EditStreadmerDtoValidator());
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
        //private readonly IStreamerRepository _streamerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<Handler> _logger;

        public Handler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<Handler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            // var streamer = await _streamerRepository.GetByIdAsync(request.Id);
            var streamer = await _unitOfWork.StreamerRepository.GetByIdAsync(request.Id);

            if (streamer is null)
            {
                _logger.LogError($"[Streamer] [Edit] NotFound {request.Id}");
                throw new NotFoundException(nameof(Streamer), request.Id);
            }

            // payload -> streamer update
            _mapper.Map(request.Payload, streamer, typeof(EditStreamerDto), typeof(Streamer));

            // await _streamerRepository.UpdateAsync(streamer);
            await _unitOfWork.StreamerRepository.UpdateAsync(streamer);
            // await _unitOfWork.CompleteAsync();
            _logger.LogInformation($"[Streamer] [Edit] {request.Id}");

            return Unit.Value;
        }
    }
}
