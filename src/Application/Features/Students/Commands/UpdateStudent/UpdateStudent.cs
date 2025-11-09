using Ardalis.GuardClauses;
using CleanContosoUniversity.Application.Common.Interfaces;

namespace CleanContosoUniversity.Application.Features.Students.Commands.UpdateStudent;

public record UpdateStudentCommand : IRequest<bool>
{
    public int Id { get; init; }

    public string FirstMidName { get; init; }
    public string LastName { get; init; }
    public DateTime EnrollmentDate { get; init; }
}

public class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public UpdateStudentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Students
            .FindAsync(new object[] { request.Id }, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.FirstMidName = request.FirstMidName;
        entity.LastName = request.LastName;
        entity.EnrollmentDate = request.EnrollmentDate;

        int id = await _context.SaveChangesAsync(cancellationToken);

        return id > 0;
    }
}