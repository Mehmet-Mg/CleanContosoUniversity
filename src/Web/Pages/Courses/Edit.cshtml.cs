using CleanContosoUniversity.Application.Features.Courses.Commands.UpdateCourse;
using CleanContosoUniversity.Application.Features.Courses.Queries.GetCourseById;
using CleanContosoUniversity.Application.Features.Departments.Queries.Common;
using CleanContosoUniversity.Application.Features.Departments.Queries.GetDepartments;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CleanContosoUniversity.Web.Pages.Courses;

public class EditModel : DepartmentNamePageModel
{
    private readonly ISender _sender;

    public EditModel(ISender sender)
    {
        _sender = sender;
    }

    [BindProperty]
    public UpdateCourseCommand Course { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var course = await _sender.Send(new GetCourseByIdQuery { CourseID = id.Value });

        if (course == null)
        {
            return NotFound();
        }

        Course = new UpdateCourseCommand
        {
            CourseID = course.CourseID,
            Title = course.Title,
            Credits = course.Credits,
            DepartmentID = course.DepartmentID
        };

        var departments = await _sender.Send(new GetDepartmentsQuery());
        PopulateDepartmentsDropDownList(departments, Course.DepartmentID);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            var departments = await _sender.Send(new GetDepartmentsQuery());
            PopulateDepartmentsDropDownList(departments, Course.DepartmentID);
            return Page();
        }

        await _sender.Send(Course);
        return RedirectToPage("./Index");
    }

    public void PopulateDepartmentsDropDownList(List<DepartmentDto>? departments, object? selectedDepartment = null)
    {
        DepartmentNameSL = new SelectList(departments ?? new List<DepartmentDto>(),
            nameof(DepartmentDto.DepartmentID),
            nameof(DepartmentDto.Name),
            selectedDepartment);
    }
}
