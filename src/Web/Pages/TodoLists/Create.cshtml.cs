using CleanContosoUniversity.Application.Common.Security;
using CleanContosoUniversity.Application.Features.TodoLists.Commands.CreateTodoList;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CleanContosoUniversity.Web.Pages.TodoLists;

[Authorize]
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
    public CreateTodoListCommand TodoList { get; set; } = default!;

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        await _sender.Send(TodoList);
        return RedirectToPage("./Index");
    }
}

