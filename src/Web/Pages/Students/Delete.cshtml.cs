using CleanContosoUniversity.Application.Features.Students.Commands.DeleteStudent;
using CleanContosoUniversity.Application.Features.Students.Queries.GetStudentById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CleanContosoUniversity.Web.Pages.Students
{
    public class DeleteModel : PageModel
    {
        private readonly ILogger<DeleteModel> _logger;
        private readonly ISender _sender;

        public DeleteModel(
                           ILogger<DeleteModel> logger,
                           ISender sender)
        {
            _logger = logger;
            _sender = sender;
        }

        [BindProperty]
        public StudentByIdDto Student { get; set; }
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            Student = await _sender.Send(new GetStudentByIdQuery
            {
                StudentId = id
            });

            if (Student == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ErrorMessage = String.Format("Delete {ID} failed. Try again", id);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _sender.Send(new GetStudentByIdQuery
            {
                StudentId = id
            });

            if (student == null)
            {
                return NotFound();
            }

            try
            {
                await _sender.Send(new DeleteStudentCommand(id.Value));
                return RedirectToPage("./Index");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, ErrorMessage);

                return RedirectToPage("./Delete",
                                     new { id, saveChangesError = true });
            }
        }
    }
}
