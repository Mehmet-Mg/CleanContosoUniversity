using CleanContosoUniversity.Application.Common.Interfaces;

namespace CleanContosoUniversity.Application.Features.Students.Queries.GetStudentById;

public record GetStudentByIdQuery : IRequest<StudentByIdDto?>
{
    public int? StudentId { get; init; }
}

public class GetStudentByIdQueryHandler : IRequestHandler<GetStudentByIdQuery, StudentByIdDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetStudentByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<StudentByIdDto?> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
    {
        var student = await _context.Students
            .ProjectTo<StudentByIdDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(m => m.ID == request.StudentId);

        return student;
    }
}