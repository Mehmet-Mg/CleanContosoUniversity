using CleanContosoUniversity.Application.Common.Interfaces;

namespace CleanContosoUniversity.Application.Features.Students.Queries.GetStudentByIdWithDetails;

public record GetStudentByIdWithDetailsQuery : IRequest<StudentWithDetailsDto?>
{
    public int? StudentId { get; init; }
}

public class GetStudentByIdWithDetailsQueryHandler : IRequestHandler<GetStudentByIdWithDetailsQuery, StudentWithDetailsDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetStudentByIdWithDetailsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<StudentWithDetailsDto?> Handle(GetStudentByIdWithDetailsQuery request, CancellationToken cancellationToken)
    {
        var student = await _context.Students
            .Include(s => s.Enrollments)
                .ThenInclude(e => e.Course)
            .AsNoTracking()
            .ProjectTo<StudentWithDetailsDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(m => m.ID == request.StudentId);

        return student;
    }
}