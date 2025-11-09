using Ardalis.GuardClauses;
using CleanContosoUniversity.Application.Common.Interfaces;

namespace CleanContosoUniversity.Application.Features.Students.Commands.DeleteStudent;

public record DeleteStudentCommand(int Id) : IRequest;

public class DeleteStudentCommandHandler : IRequestHandler<DeleteStudentCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteStudentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Students
            .Where(s => s.ID == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        _context.Students.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}