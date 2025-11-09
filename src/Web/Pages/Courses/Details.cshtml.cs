using CleanContosoUniversity.Application.Features.Courses.Queries.GetCourseById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CleanContosoUniversity.Web.Pages.Courses;

public class DetailsModel : PageModel
{
    private readonly ISender _sender;

    public DetailsModel(ISender sender)
    {
        _sender = sender;
    }

    public CourseDetailDto Course { get; set; } = default!;

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

        Course = course;
        return Page();
    }
}
