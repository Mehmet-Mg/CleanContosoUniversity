using CleanContosoUniversity.Application.Features.Departments.Queries.Common;
using CleanContosoUniversity.Application.Features.Departments.Queries.GetDepartments;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CleanContosoUniversity.Web.Pages.Departments;

public class IndexModel : PageModel
{
    private readonly ISender _sender;

    public IndexModel(ISender sender)
    {
        _sender = sender;
    }

    public IList<DepartmentDto> Department { get;set; } = default!;

    public async Task OnGetAsync()
    {
        Department = await _sender.Send(new GetDepartmentsQuery());
    }
}
