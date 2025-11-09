using CleanContosoUniversity.Application.Features.TodoItems.Commands.CreateTodoItem;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CleanContosoUniversity.Web.Pages.TodoItems;

public class CreateModel : PageModel
{
    private readonly ISender _sender;

    public CreateModel(ISender sender)
    {
        _sender = sender;
    }

    public IActionResult OnGet(int? listId)
    {
        if (!listId.HasValue)
        {
            return RedirectToPage("../TodoLists/Index");
        }

        TodoItem = new CreateTodoItemCommand
        {
            ListId = listId.Value
        };

        return Page();
    }

    [BindProperty]
    public CreateTodoItemCommand TodoItem { get; set; } = default!;

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        await _sender.Send(TodoItem);
        return RedirectToPage("./Index", new { listId = TodoItem.ListId });
    }
}

