using Ardalis.GuardClauses;
using CleanContosoUniversity.Application.Common.Interfaces;

namespace CleanContosoUniversity.Application.Features.Courses.Commands.DeleteCourse;

public record DeleteCourseCommand(int CourseID) : IRequest;

public class DeleteCourseCommandHandler : IRequestHandler<DeleteCourseCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteCourseCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Courses
            .FindAsync(new object[] { request.CourseID }, cancellationToken);

        Guard.Against.NotFound(request.CourseID, entity);

        _context.Courses.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

