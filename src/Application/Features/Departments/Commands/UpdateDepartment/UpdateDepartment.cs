using CleanContosoUniversity.Application.Common.Interfaces;

namespace CleanContosoUniversity.Application.Features.Departments.Commands.UpdateDepartment;

public record UpdateDepartmentCommand : IRequest<int>
{
    public int DepartmentId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Budget { get; set; }
    public DateTime StartDate { get; set; }
    public int? InstructorID { get; set; }
    public byte[] ConcurrencyToken { get; set; } = Array.Empty<byte>();
}

public class UpdateDepartmentCommandHandler : IRequestHandler<UpdateDepartmentCommand, int>
{
    private readonly IApplicationDbContext _context;

    public UpdateDepartmentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Departments
            .Include(d => d.Administrator)
            .FirstOrDefaultAsync(d => d.DepartmentID == request.DepartmentId, cancellationToken);

        Guard.Against.NotFound(request.DepartmentId, entity);

        // Set ConcurrencyToken original value for concurrency check
        if (request.ConcurrencyToken != null && request.ConcurrencyToken.Length > 0)
        {
            _context.Departments.Entry(entity).Property(d => d.ConcurrencyToken).OriginalValue = request.ConcurrencyToken;
        }

        entity.Name = request.Name;
        entity.Budget = request.Budget;
        entity.StartDate = request.StartDate;
        entity.InstructorID = request.InstructorID;

        await _context.SaveChangesAsync(cancellationToken);

        return entity.DepartmentID;
    }
}