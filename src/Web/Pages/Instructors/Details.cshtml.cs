using CleanContosoUniversity.Application.Features.Instructors.Queries.GetInstructorById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CleanContosoUniversity.Web.Pages.Instructors;

public class DetailsModel : PageModel
{
    private readonly ISender _sender;

    public DetailsModel(ISender sender)
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
}
