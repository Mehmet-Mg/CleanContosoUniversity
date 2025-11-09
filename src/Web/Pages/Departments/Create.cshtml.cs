using CleanContosoUniversity.Application.Features.Departments.Commands.CreateDepartment;
using CleanContosoUniversity.Application.Features.Instructors.Queries.GetInstructors;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CleanContosoUniversity.Web.Pages.Departments;

public class CreateModel : PageModel
{
    private readonly ISender _sender;

    public CreateModel(ISender sender)
    {
        _sender = sender;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var instructors = await _sender.Send(new GetInstructorsQuery());
        ViewData["InstructorID"] = new SelectList(instructors ?? new List<InstructoDto>(), "ID", "FullName");
        return Page();
    }

    public class InputModel
    {
        public string Name { get; set; } = default!;
        public decimal Budget { get; set; }
        public DateTime StartDate { get; set; }
        public int? InstructorID { get; set; }
    }

    [BindProperty]
    public InputModel Input { get; set; } = default!;

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            var instructors = await _sender.Send(new GetInstructorsQuery());
            ViewData["InstructorID"] = new SelectList(instructors ?? new List<InstructoDto>(), "ID", "FullName", Input.InstructorID);
            return Page();
        }

        await _sender.Send(new CreateDepartmentCommand
        {
            Budget = Input.Budget,
            InstructorID = Input.InstructorID,
            Name = Input.Name,
            StartDate = Input.StartDate
        });

        return RedirectToPage("./Index");
    }
}
