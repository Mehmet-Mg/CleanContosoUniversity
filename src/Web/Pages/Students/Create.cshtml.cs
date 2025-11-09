using CleanContosoUniversity.Application.Features.Students.Commands.CreateStudent;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CleanContosoUniversity.Web.Pages.Students
{
    public class CreateModel : PageModel
    {
        private readonly ISender _sender;

        public CreateModel(ISender sender)
        {
            _sender = sender;
        }

        public IActionResult OnGet()
        {
            return Page();
        }


        [BindProperty]
        public CreateStudentCommand Student { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _sender.Send(Student);
            return RedirectToPage("./Index");
        }
    }
}
