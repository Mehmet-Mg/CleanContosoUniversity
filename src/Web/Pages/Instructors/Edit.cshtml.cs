using CleanContosoUniversity.Application.Features.Courses.Queries.GetCoursesForInstructor;
using CleanContosoUniversity.Application.Features.Instructors.Commands.UpdateInstructor;
using CleanContosoUniversity.Application.Features.Instructors.Queries.GetInstructorById;
using CleanContosoUniversity.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanContosoUniversity.Web.Pages.Instructors;

public class EditModel : InstructorCoursesPageModel
{
    private readonly ISender _sender;

    public EditModel(ISender sender)
    {
        _sender = sender;
    }

    [BindProperty]
    public UpdateInstructorCommand Instructor { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var instructor = await _sender.Send(new GetInstructorByIdQuery { InstructorID = id });

        if (instructor == null)
        {
            return NotFound();
        }

        Instructor = new UpdateInstructorCommand
        {
            ID = instructor.ID,
            LastName = instructor.LastName,
            FirstMidName = instructor.FirstMidName,
            HireDate = instructor.HireDate,
            OfficeLocation = instructor.OfficeAssignment?.Location,
            SelectedCourses = instructor.Courses.Select(c => c.CourseID).ToArray()
        };

        var courses = await _sender.Send(new GetCoursesForInstructorQuery());
        PopulateAssignedCourseData(courses, Instructor.SelectedCourses ?? Array.Empty<int>());
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            var courses = await _sender.Send(new GetCoursesForInstructorQuery());
            PopulateAssignedCourseData(courses, Instructor.SelectedCourses ?? Array.Empty<int>());
            return Page();
        }

        await _sender.Send(Instructor);
        return RedirectToPage("./Index");
    }

    public void PopulateAssignedCourseData(List<CourseForInstructorDto> allCourses, int[] instructorCourses)
    {
        var instructorCoursesHS = new HashSet<int>(instructorCourses);
        AssignedCourseDataList = new List<AssignedCourseData>();
        
        foreach (var course in allCourses)
        {
            AssignedCourseDataList.Add(new AssignedCourseData
            {
                CourseID = course.CourseID,
                Title = course.Title,
                Assigned = instructorCoursesHS.Contains(course.CourseID)
            });
        }
    }
}
