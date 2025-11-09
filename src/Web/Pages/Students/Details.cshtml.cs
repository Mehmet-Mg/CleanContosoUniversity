using CleanContosoUniversity.Application.Features.Students.Queries.GetStudentByIdWithDetails;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CleanContosoUniversity.Web.Pages.Students
{
    public class DetailsModel : PageModel
    {
        private readonly ISender _sender;

        public DetailsModel(ISender sender)
        {
            _sender = sender;
        }

        public StudentWithDetailsDto Student { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _sender.Send(new GetStudentByIdWithDetailsQuery
            {
                StudentId = id
            });

            if (student is not null)
            {
                Student = student;

                return Page();
            }

            return NotFound();
        }
    }
}
