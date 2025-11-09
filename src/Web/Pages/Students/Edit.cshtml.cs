using CleanContosoUniversity.Application.Features.Students.Commands.UpdateStudent;
using CleanContosoUniversity.Application.Features.Students.Queries.GetStudentById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CleanContosoUniversity.Web.Pages.Students
{
    public class EditModel : PageModel
    {
        private readonly ISender _sender;

        public EditModel(ISender sender)
        {
            _sender = sender;
        }

        [BindProperty]
        public StudentByIdDto Student { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
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
            Student = student;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int id)
        {
            var studentToUpdate = await _sender.Send(new GetStudentByIdQuery
            {
                StudentId = id
            });

            if (studentToUpdate == null)
            {
                return NotFound();
            }

            var command = new UpdateStudentCommand
            {
                Id = id,
                LastName = Student.LastName,
                FirstMidName = Student.FirstMidName,
                EnrollmentDate = Student.EnrollmentDate
            };

            // 2. TryUpdateModelAsync ile HTTP İstek Verilerini Komuta Bağlayın
            bool success = await TryUpdateModelAsync(command);

            // 3. Model Doğrulamasını Kontrol Edin
            if (!success || !ModelState.IsValid)
            {
                return Page();
            }

            var isUpdated = await _sender.Send(command);

            if (isUpdated)
            {
                return RedirectToPage("./Index");
            }

            return Page();
        }
    }
}
