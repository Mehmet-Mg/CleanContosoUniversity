using CleanContosoUniversity.Application.Features.TodoLists.Queries.GetTodoListById;
using CleanContosoUniversity.Application.Common.Models;
using CleanContosoUniversity.Application.Features.TodoItems.Queries.GetTodoItemsWithPagination;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CleanContosoUniversity.Web.Pages.TodoItems;

public class IndexModel : PageModel
{
    private readonly ISender _sender;

    public IndexModel(ISender sender)
    {
        _sender = sender;
    }

    public PaginatedList<TodoItemBriefDto> TodoItems { get; set; } = default!;
    public int? ListId { get; set; }
    public string? ListTitle { get; set; }

    public async Task OnGetAsync(int? listId, int? pageIndex)
    {
        ListId = listId;

        if (listId.HasValue)
        {
            var todoList = await _sender.Send(new GetTodoListByIdQuery(listId.Value));
            ListTitle = todoList?.Title;

            TodoItems = await _sender.Send(new GetTodoItemsWithPaginationQuery
            {
                ListId = listId.Value,
                PageNumber = pageIndex ?? 1,
                PageSize = 10
            });
        }
        else
        {
            TodoItems = new PaginatedList<TodoItemBriefDto>(new List<TodoItemBriefDto>(), 0, 1, 10);
        }
    }
}

