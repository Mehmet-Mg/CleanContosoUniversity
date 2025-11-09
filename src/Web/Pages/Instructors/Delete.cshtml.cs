using CleanContosoUniversity.Application.Features.Instructors.Commands.DeleteInstructor;
using CleanContosoUniversity.Application.Features.Instructors.Queries.GetInstructorById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CleanContosoUniversity.Web.Pages.Instructors;

public class DeleteModel : PageModel
{
    private readonly ISender _sender;

    public DeleteModel(ISender sender)
    {
        _sender = sender;
    }

    public InstructorCoursesDto Instructor { get; set; } = default!;

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

        Instructor = instructor;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        await _sender.Send(new DeleteInstructorCommand(id.Value));
        return RedirectToPage("./Index");
    }
}
