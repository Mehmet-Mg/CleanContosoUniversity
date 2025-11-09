using CleanContosoUniversity.Application.Features.Courses.Queries.GetCourses;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CleanContosoUniversity.Web.Pages.Courses;

public class IndexModel : PageModel
{
    private readonly ISender _sender;

    public IndexModel(ISender sender)
    {
        _sender = sender;
    }

    public IList<CourseDto> CourseVM { get; set; } = default!;

    public async Task OnGetAsync()
    {
        CourseVM = await _sender.Send(new GetCoursesQuery());
    }
}
