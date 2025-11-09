using CleanContosoUniversity.Application.Features.Departments.Queries.GetDepartmentWithDetailsById;
using CleanContosoUniversity.Application.Features.Departments.Queries.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CleanContosoUniversity.Web.Pages.Departments;

public class DetailsModel : PageModel
{
    private readonly ISender _sender;

    public DetailsModel(ISender sender)
    {
        _sender = sender;
    }

    public DepartmentDto Department { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var department = await _sender.Send(new GetDepartmentWithDetailByIdQuery
        {
            DepartmentId = id.Value
        });

        if (department is not null)
        {
            Department = department;
             
            return Page();
        }

        return NotFound();
    }
}
