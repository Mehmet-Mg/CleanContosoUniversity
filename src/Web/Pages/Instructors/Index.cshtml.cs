using CleanContosoUniversity.Application.Features.Enrollments.Queries.GetEnrollmentsByCourseId;
using CleanContosoUniversity.Application.Features.Instructors.Queries.GetInstructors;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CleanContosoUniversity.Web.Pages.Instructors;

public class IndexModel : PageModel
{
    private readonly ISender _sender;

    public IndexModel(ISender sender)
    {
        _sender = sender;
    }

    public class InstructorIndexData
    {
        public IEnumerable<InstructoDto> Instructors { get; set; } = Enumerable.Empty<InstructoDto>();
        public IEnumerable<CourseDto> Courses { get; set; } = Enumerable.Empty<CourseDto>();
        public IEnumerable<CourseEnrollmentDto> Enrollments { get; set; } = Enumerable.Empty<CourseEnrollmentDto>();
    }

    public InstructorIndexData InstructorData { get; set; } = new();
    public int InstructorID { get; set; }
    public int CourseID { get; set; }

    public async Task OnGetAsync(int? id, int? courseID)
    {
        InstructorData = new InstructorIndexData();
        var instructors = await _sender.Send(new GetInstructorsQuery());
        InstructorData.Instructors = instructors ?? new List<InstructoDto>();

        if (id != null)
        {
            InstructorID = id.Value;
            var instructor = InstructorData.Instructors.FirstOrDefault(i => i.ID == id.Value);
            if (instructor != null)
            {
                InstructorData.Courses = instructor.Courses.Select(c => new CourseDto
                {
                    CourseID = c.CourseID,
                    Title = c.Title,
                    Credits = c.Credits,
                    DepartmentName = c.Department?.Name ?? string.Empty
                });
            }
        }

        if (courseID != null)
        {
            CourseID = courseID.Value;
            InstructorData.Enrollments = await _sender.Send(new GetEnrollmentByCourseIdQuery
            {
                CourseID = courseID.Value,
            });
        }
    }
}

public class CourseDto
{
    public int CourseID { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Credits { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
}

//public class EnrollmentDto
//{
//    public int EnrollmentID { get; set; }
//    public int CourseID { get; set; }
//    public int StudentID { get; set; }
//    public string StudentName { get; set; } = string.Empty;
//    public Grade? Grade { get; set; }
//}
