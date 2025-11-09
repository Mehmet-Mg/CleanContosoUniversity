using CleanContosoUniversity.Application.Common.Security;
using CleanContosoUniversity.Application.Features.TodoLists.Queries.GetTodos;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CleanContosoUniversity.Web.Pages.TodoLists;

[Authorize]
public class IndexModel : PageModel
{
    private readonly ISender _sender;

    public IndexModel(ISender sender)
    {
        _sender = sender;
    }

    public TodosVm Todos { get; set; } = default!;

    public async Task OnGetAsync()
    {
        Todos = await _sender.Send(new GetTodosQuery());
    }
}

