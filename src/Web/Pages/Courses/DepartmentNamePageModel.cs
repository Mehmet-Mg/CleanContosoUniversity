using CleanContosoUniversity.Application.Features.Departments.Queries.Common;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CleanContosoUniversity.Web.Pages.Courses;

public class DepartmentNamePageModel : PageModel
{
    public SelectList DepartmentNameSL { get; set; } = default!;

    public void PopulateDepartmentsDropDownList(List<DepartmentDto>? departments, object? selectedDepartment = null)
    {
        DepartmentNameSL = new SelectList(departments ?? new List<DepartmentDto>(),
            nameof(DepartmentDto.DepartmentID),
            nameof(DepartmentDto.Name),
            selectedDepartment);
    }
}
