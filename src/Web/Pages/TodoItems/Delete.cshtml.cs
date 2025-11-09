using CleanContosoUniversity.Application.Features.TodoItems.Commands.DeleteTodoItem;
using CleanContosoUniversity.Application.Features.TodoItems.Queries.GetTodoItemById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CleanContosoUniversity.Web.Pages.TodoItems;

public class DeleteModel : PageModel
{
    private readonly ISender _sender;

    public DeleteModel(ISender sender)
    {
        _sender = sender;
    }

    public TodoItemDto TodoItem { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var todoItem = await _sender.Send(new GetTodoItemByIdQuery(id));

        if (todoItem == null)
        {
            return NotFound();
        }

        TodoItem = todoItem;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var todoItem = await _sender.Send(new GetTodoItemByIdQuery(id));
        if (todoItem == null)
        {
            return NotFound();
        }

        await _sender.Send(new DeleteTodoItemCommand(id));

        return RedirectToPage("./Index", new { listId = todoItem.ListId });
    }
}

