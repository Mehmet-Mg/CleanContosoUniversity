using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanContosoUniversity.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CleanContosoUniversity.Application.Features.TodoItems.Queries.GetTodoItemById;

public record GetTodoItemByIdQuery(int Id) : IRequest<TodoItemDto?>;

public class GetTodoItemByIdQueryHandler : IRequestHandler<GetTodoItemByIdQuery, TodoItemDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTodoItemByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<TodoItemDto?> Handle(GetTodoItemByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.TodoItems
            .AsNoTracking()
            .Where(t => t.Id == request.Id)
            .ProjectTo<TodoItemDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }
}

