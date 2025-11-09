using CleanContosoUniversity.Application.Common.Security;
using CleanContosoUniversity.Application.Features.TodoLists.Commands.UpdateTodoList;
using CleanContosoUniversity.Application.Features.TodoLists.Queries.GetTodoListById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CleanContosoUniversity.Web.Pages.TodoLists;

[Authorize]
public class EditModel : PageModel
{
    private readonly ISender _sender;

    public EditModel(ISender sender)
    {
        _sender = sender;
    }

    [BindProperty]
    public UpdateTodoListCommand TodoList { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var todoList = await _sender.Send(new GetTodoListByIdQuery(id));

        if (todoList == null)
        {
            return NotFound();
        }

        TodoList = new UpdateTodoListCommand
        {
            Id = todoList.Id,
            Title = todoList.Title,
            Colour = todoList.Colour
        };

        return Page();
    }

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

