using CleanContosoUniversity.Application.Features.Departments.Commands.DeleteDepartment;
using CleanContosoUniversity.Application.Features.Departments.Queries.GetDepartmentWithDetailsById;
using CleanContosoUniversity.Application.Features.Departments.Queries.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CleanContosoUniversity.Web.Pages.Departments;

public class DeleteModel : PageModel
{
    private readonly ISender _sender;

    public DeleteModel(ISender sender)
    {
        _sender = sender;
    }

    [BindProperty]
    public DepartmentDto? Department { get; set; }
    public string ConcurrencyErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id, bool? concurrencyError)
    {
        if (id == null)
        {
            return NotFound();
        }

        Department = await _sender.Send(new GetDepartmentWithDetailByIdQuery
        {
            DepartmentId = id.Value
        });

        if (Department == null)
        {
            return NotFound();
        }

        if (concurrencyError.GetValueOrDefault())
        {
            ConcurrencyErrorMessage = "The record you attempted to delete "
              + "was modified by another user after you selected delete. "
              + "The delete operation was canceled and the current values in the "
              + "database have been displayed. If you still want to delete this "
              + "record, click the Delete button again.";
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        try
        {
            await _sender.Send(new DeleteDepartmentCommand(id));
            return RedirectToPage("./Index");
        }
        catch (DbUpdateConcurrencyException)
        {
            return RedirectToPage("./Delete",
                new { concurrencyError = true, id = id });
        }
    }
}
