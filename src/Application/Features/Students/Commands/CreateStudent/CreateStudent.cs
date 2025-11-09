using CleanContosoUniversity.Application.Common.Interfaces;
using CleanContosoUniversity.Domain.Entities;

namespace CleanContosoUniversity.Application.Features.Students.Commands.CreateStudent;

public record CreateStudentCommand : IRequest<int>
{
    public string LastName { get; set; }
    public string FirstMidName { get; set; }
    public DateTime EnrollmentDate { get; set; }
}

public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateStudentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
    {
        var entity = new Student();

        entity.FirstMidName = request.FirstMidName;
        entity.LastName = request.LastName;
        entity.EnrollmentDate = request.EnrollmentDate;

        _context.Students.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.ID;
    }
}