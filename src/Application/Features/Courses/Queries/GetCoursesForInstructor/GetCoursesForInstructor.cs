using CleanContosoUniversity.Application.Common.Interfaces;

namespace CleanContosoUniversity.Application.Features.Courses.Queries.GetCoursesForInstructor;

public record GetCoursesForInstructorQuery : IRequest<List<CourseForInstructorDto>>;

public class GetCoursesForInstructorQueryHandler : IRequestHandler<GetCoursesForInstructorQuery, List<CourseForInstructorDto>>
{
    private readonly IApplicationDbContext _context;

    public GetCoursesForInstructorQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<CourseForInstructorDto>> Handle(GetCoursesForInstructorQuery request, CancellationToken cancellationToken)
    {
        return await _context.Courses
            .AsNoTracking()
            .Select(c => new CourseForInstructorDto
            {
                CourseID = c.CourseID,
                Title = c.Title
            })
            .OrderBy(c => c.Title)
            .ToListAsync(cancellationToken);
    }
}

public class CourseForInstructorDto
{
    public int CourseID { get; set; }
    public string Title { get; set; } = string.Empty;
}

