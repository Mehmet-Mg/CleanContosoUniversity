using Ardalis.GuardClauses;
using CleanContosoUniversity.Application.Common.Interfaces;

namespace CleanContosoUniversity.Application.Features.Instructors.Commands.DeleteInstructor;

public record DeleteInstructorCommand(int ID) : IRequest;

public class DeleteInstructorCommandHandler : IRequestHandler<DeleteInstructorCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteInstructorCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteInstructorCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Instructors
            .Include(i => i.Courses)
            .FirstOrDefaultAsync(i => i.ID == request.ID, cancellationToken);

        Guard.Against.NotFound(request.ID, entity);

        // Remove instructor from departments
        var departments = await _context.Departments
            .Where(d => d.InstructorID == request.ID)
            .ToListAsync(cancellationToken);
        
        foreach (var department in departments)
        {
            department.InstructorID = null;
        }

        _context.Instructors.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

