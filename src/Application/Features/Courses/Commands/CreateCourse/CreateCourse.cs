using CleanContosoUniversity.Application.Common.Interfaces;
using CleanContosoUniversity.Domain.Entities;

namespace CleanContosoUniversity.Application.Features.Courses.Commands.CreateCourse;

public record CreateCourseCommand : IRequest<int>
{
    public int CourseID { get; init; }
    public string Title { get; init; } = string.Empty;
    public int Credits { get; init; }
    public int DepartmentID { get; init; }
}

public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateCourseCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        var entity = new Course
        {
            CourseID = request.CourseID,
            Title = request.Title,
            Credits = request.Credits,
            DepartmentID = request.DepartmentID
        };

        _context.Courses.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.CourseID;
    }
}

