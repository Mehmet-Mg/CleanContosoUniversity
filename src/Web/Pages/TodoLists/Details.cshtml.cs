using CleanContosoUniversity.Application.Common.Security;
using CleanContosoUniversity.Application.Features.TodoLists.Queries.GetTodoListById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CleanContosoUniversity.Web.Pages.TodoLists;

[Authorize]
public class DetailsModel : PageModel
{
    private readonly ISender _sender;

    public DetailsModel(ISender sender)
    {
        _sender = sender;
    }

    public TodoListDto TodoList { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var todoList = await _sender.Send(new GetTodoListByIdQuery(id));

        if (todoList == null)
        {
            return NotFound();
        }

        TodoList = todoList;
        return Page();
    }
}

