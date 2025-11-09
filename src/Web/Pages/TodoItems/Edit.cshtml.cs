using CleanContosoUniversity.Application.Features.TodoItems.Queries.GetTodoItemById;
using CleanContosoUniversity.Application.Features.TodoItems.Commands.UpdateTodoItem;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CleanContosoUniversity.Web.Pages.TodoItems;

public class EditModel : PageModel
{
    private readonly ISender _sender;

    public EditModel(ISender sender)
    {
        _sender = sender;
    }

    [BindProperty]
    public UpdateTodoItemCommand TodoItem { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var todoItem = await _sender.Send(new GetTodoItemByIdQuery(id));

        if (todoItem == null)
        {
            return NotFound();
        }

        TodoItem = new UpdateTodoItemCommand
        {
            Id = todoItem.Id,
            Title = todoItem.Title,
            Done = todoItem.Done
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var todoItem = await _sender.Send(new GetTodoItemByIdQuery(TodoItem.Id));
        if (todoItem == null)
        {
            return NotFound();
        }

        await _sender.Send(TodoItem);

        return RedirectToPage("./Index", new { listId = todoItem.ListId });
    }
}

