using CleanContosoUniversity.Application.Common.Interfaces;

namespace CleanContosoUniversity.Application.Features.Courses.Commands.UpdateCourse;

public record UpdateCourseCommand : IRequest
{
    public int CourseID { get; init; }
    public string Title { get; init; } = string.Empty;
    public int Credits { get; init; }
    public int DepartmentID { get; init; }
}

public class UpdateCourseCommandHandler : IRequestHandler<UpdateCourseCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateCourseCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Courses
            .FindAsync(new object[] { request.CourseID }, cancellationToken);

        Guard.Against.NotFound(request.CourseID, entity);

        entity.Title = request.Title;
        entity.Credits = request.Credits;
        entity.DepartmentID = request.DepartmentID;

        await _context.SaveChangesAsync(cancellationToken);
    }
}

