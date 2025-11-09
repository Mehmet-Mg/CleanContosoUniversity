using CleanContosoUniversity.Application.Features.Courses.Commands.CreateCourse;
using CleanContosoUniversity.Application.Features.Departments.Queries.Common;
using CleanContosoUniversity.Application.Features.Departments.Queries.GetDepartments;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CleanContosoUniversity.Web.Pages.Courses;

public class CreateModel : DepartmentNamePageModel
{
    private readonly ISender _sender;

    public CreateModel(ISender sender)
    {
        _sender = sender;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var departments = await _sender.Send(new GetDepartmentsQuery());
        PopulateDepartmentsDropDownList(departments);
        return Page();
    }

    [BindProperty]
    public CreateCourseCommand Course { get; set; } = default!;

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
