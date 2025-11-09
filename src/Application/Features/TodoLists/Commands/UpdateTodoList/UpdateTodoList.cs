using Ardalis.GuardClauses;
using CleanContosoUniversity.Application.Common.Interfaces;
using CleanContosoUniversity.Domain.ValueObjects;

namespace CleanContosoUniversity.Application.Features.TodoLists.Commands.UpdateTodoList;

public record UpdateTodoListCommand : IRequest
{
    public int Id { get; init; }

    public string? Title { get; init; }
    
    public string? Colour { get; init; }
}

public class UpdateTodoListCommandHandler : IRequestHandler<UpdateTodoListCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateTodoListCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateTodoListCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.TodoLists
            .FindAsync(new object[] { request.Id }, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.Title = request.Title;
        
        if (!string.IsNullOrWhiteSpace(request.Colour))
        {
            entity.Colour = Colour.From(request.Colour);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}