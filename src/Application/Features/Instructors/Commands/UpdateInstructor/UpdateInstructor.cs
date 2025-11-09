using CleanContosoUniversity.Application.Common.Interfaces;
using CleanContosoUniversity.Domain.Entities;

namespace CleanContosoUniversity.Application.Features.Instructors.Commands.UpdateInstructor;

public record UpdateInstructorCommand : IRequest
{
    public int ID { get; init; }
    public string LastName { get; init; } = string.Empty;
    public string FirstMidName { get; init; } = string.Empty;
    public DateTime HireDate { get; init; }
    public string? OfficeLocation { get; init; }
    public int[]? SelectedCourses { get; init; }
}

public class UpdateInstructorCommandHandler : IRequestHandler<UpdateInstructorCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateInstructorCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateInstructorCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Instructors
            .Include(i => i.OfficeAssignment)
            .Include(i => i.Courses)
            .FirstOrDefaultAsync(i => i.ID == request.ID, cancellationToken);

        Guard.Against.NotFound(request.ID, entity);

        entity.LastName = request.LastName;
        entity.FirstMidName = request.FirstMidName;
        entity.HireDate = request.HireDate;

        if (string.IsNullOrWhiteSpace(request.OfficeLocation))
        {
            entity.OfficeAssignment = null;
        }
        else
        {
            if (entity.OfficeAssignment == null)
            {
                entity.OfficeAssignment = new OfficeAssignment();
            }
            entity.OfficeAssignment.Location = request.OfficeLocation;
        }

        UpdateInstructorCourses(request.SelectedCourses, entity);

        await _context.SaveChangesAsync(cancellationToken);
    }

    private void UpdateInstructorCourses(int[]? selectedCourses, Instructor instructor)
    {
        if (selectedCourses == null)
        {
            instructor.Courses = new List<Course>();
            return;
        }

        var selectedCoursesHS = new HashSet<int>(selectedCourses);
        var instructorCourses = new HashSet<int>(instructor.Courses.Select(c => c.CourseID));

        var allCourses = _context.Courses.ToList();

        foreach (var course in allCourses)
        {
            if (selectedCoursesHS.Contains(course.CourseID))
            {
                if (!instructorCourses.Contains(course.CourseID))
                {
                    instructor.Courses.Add(course);
                }
            }
            else
            {
                if (instructorCourses.Contains(course.CourseID))
                {
                    var courseToRemove = instructor.Courses.Single(c => c.CourseID == course.CourseID);
                    instructor.Courses.Remove(courseToRemove);
                }
            }
        }
    }
}

