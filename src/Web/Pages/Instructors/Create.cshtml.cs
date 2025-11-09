using CleanContosoUniversity.Application.Features.Courses.Queries.GetCoursesForInstructor;
using CleanContosoUniversity.Application.Features.Instructors.Commands.CreateInstructor;
using CleanContosoUniversity.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CleanContosoUniversity.Web.Pages.Instructors;

public class CreateModel : InstructorCoursesPageModel
{
    private readonly ISender _sender;

    public CreateModel(ISender sender)
    {
        _sender = sender;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var instructor = new CreateInstructorCommand
        {
            SelectedCourses = Array.Empty<string>()
        };

        var courses = await _sender.Send(new GetCoursesForInstructorQuery());
        PopulateAssignedCourseData(courses, instructor.SelectedCourses ?? Array.Empty<string>());
        return Page();
    }

    [BindProperty]
    public CreateInstructorCommand Instructor { get; set; } = default!;

    public async Task<IActionResult> OnPostAsync(string[] selectedCourses)
    {
        if (!ModelState.IsValid)
        {
            var courses = await _sender.Send(new GetCoursesForInstructorQuery());
            PopulateAssignedCourseData(courses, Instructor.SelectedCourses ?? Array.Empty<string>());
            return Page();
        }

        Instructor.SelectedCourses = selectedCourses;

        await _sender.Send(Instructor);
        return RedirectToPage("./Index");
    }

    public void PopulateAssignedCourseData(List<CourseForInstructorDto> allCourses, string[] instructorCourses)
    {
        var instructorCoursesHS = new HashSet<string>(instructorCourses);
        AssignedCourseDataList = new List<AssignedCourseData>();
        
        foreach (var course in allCourses)
        {
            AssignedCourseDataList.Add(new AssignedCourseData
            {
                CourseID = course.CourseID,
                Title = course.Title,
                Assigned = instructorCoursesHS.Contains(course.CourseID.ToString())
            });
        }
    }
}
