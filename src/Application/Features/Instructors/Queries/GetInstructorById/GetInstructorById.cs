using CleanContosoUniversity.Application.Common.Interfaces;

namespace CleanContosoUniversity.Application.Features.Instructors.Queries.GetInstructorById;

public record GetInstructorByIdQuery : IRequest<InstructorCoursesDto?>
{
    public int? InstructorID { get; init; }
}

public class GetInstructorByIdHandler : IRequestHandler<GetInstructorByIdQuery, InstructorCoursesDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetInstructorByIdHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<InstructorCoursesDto?> Handle(GetInstructorByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Instructors
            .Include(i => i.OfficeAssignment)
            .Include(i => i.Courses)
                .ThenInclude(c => c.Department)
            .AsNoTracking()
            .ProjectTo<InstructorCoursesDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(i => i.ID == request.InstructorID, cancellationToken);
    }
}