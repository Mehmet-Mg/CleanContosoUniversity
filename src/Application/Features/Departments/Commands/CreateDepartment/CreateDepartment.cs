using CleanContosoUniversity.Application.Common.Interfaces;
using CleanContosoUniversity.Domain.Entities;

namespace CleanContosoUniversity.Application.Features.Departments.Commands.CreateDepartment;

public record CreateDepartmentCommand : IRequest<int>
{
    public string Name { get; init; } = string.Empty;
    public decimal Budget { get; init; }
    public DateTime StartDate { get; init; }
    public int? InstructorID { get; init; }
}

public class CreateDepartmentCommandHandler : IRequestHandler<CreateDepartmentCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateDepartmentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
    {
        var entity = new Department
        {
            Name = request.Name,
            Budget = request.Budget,
            StartDate = request.StartDate,
            InstructorID = request.InstructorID
        };

        _context.Departments.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.DepartmentID;
    }
}