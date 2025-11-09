using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanContosoUniversity.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CleanContosoUniversity.Application.Features.TodoLists.Queries.GetTodoListById;

public record GetTodoListByIdQuery(int Id) : IRequest<TodoListDto?>;

public class GetTodoListByIdQueryHandler : IRequestHandler<GetTodoListByIdQuery, TodoListDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTodoListByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<TodoListDto?> Handle(GetTodoListByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.TodoLists
            .AsNoTracking()
            .Where(t => t.Id == request.Id)
            .ProjectTo<TodoListDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }
}

