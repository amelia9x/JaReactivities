using Application.Activities;
using Application.Core;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application
{
    public class Create
    {
        public class Command : IRequest<Result<Unit>> {
            public Activity Activity { get; set; }            
        }

        public class CommandValidator : AbstractValidator<Command> {
            public CommandValidator() {
                RuleFor(x => x.Activity).SetValidator(new ActivityValidator());
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context) {
                _context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                _context.Activities.Add(request.Activity);
                await _context.SaveChangesAsync();
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}