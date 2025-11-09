using CleanContosoUniversity.Application.Common.Interfaces;

namespace CleanContosoUniversity.Application.Features.Enrollments.Queries.GetEnrollmentsByCourseId;

public record GetEnrollmentByCourseIdQuery : IRequest<List<CourseEnrollmentDto>>
{
    public int CourseID { get; init; }
}

public class GetEnrollmentByCourseIdQueryHandler : IRequestHandler<GetEnrollmentByCourseIdQuery, List<CourseEnrollmentDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetEnrollmentByCourseIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<CourseEnrollmentDto>> Handle(GetEnrollmentByCourseIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Enrollments
            .Include(c => c.Course)
            .AsNoTracking()
            .ProjectTo<CourseEnrollmentDto>(_mapper.ConfigurationProvider)
            .Where(c => c.CourseID == request.CourseID)
            .ToListAsync();
    }
}
