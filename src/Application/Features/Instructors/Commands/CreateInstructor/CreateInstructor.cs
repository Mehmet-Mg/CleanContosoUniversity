using CleanContosoUniversity.Application.Common.Interfaces;
using CleanContosoUniversity.Domain.Entities;

namespace CleanContosoUniversity.Application.Features.Instructors.Commands.CreateInstructor;

public record CreateInstructorCommand : IRequest<int>
{
    public string LastName { get; init; } = string.Empty;
    public string FirstMidName { get; init; } = string.Empty;
    public DateTime HireDate { get; init; }
    public string? OfficeLocation { get; init; }
    public string[]? SelectedCourses { get; set; }
}

public class CreateInstructorCommandHandler : IRequestHandler<CreateInstructorCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateInstructorCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateInstructorCommand request, CancellationToken cancellationToken)
    {
        var entity = new Instructor
        {
            LastName = request.LastName,
            FirstMidName = request.FirstMidName,
            HireDate = request.HireDate
        };

        if (!string.IsNullOrWhiteSpace(request.OfficeLocation))
        {
            entity.OfficeAssignment = new OfficeAssignment
            {
                Location = request.OfficeLocation
            };
        }

        if (request.SelectedCourses != null && request.SelectedCourses.Length > 0)
        {
            entity.Courses = new List<Course>();
            foreach (var courseId in request.SelectedCourses)
            {
                var course = await _context.Courses.FindAsync(new object[] { Convert.ToInt32(courseId) }, cancellationToken);
                if (course != null)
                {
                    entity.Courses.Add(course);
                }
            }
        }

        _context.Instructors.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.ID;
    }
}

