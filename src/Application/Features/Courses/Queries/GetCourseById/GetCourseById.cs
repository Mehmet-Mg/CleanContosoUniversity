using CleanContosoUniversity.Application.Common.Interfaces;

namespace CleanContosoUniversity.Application.Features.Courses.Queries.GetCourseById;

public record GetCourseByIdQuery : IRequest<CourseDetailDto?>
{
    public int CourseID { get; init; }
}

public class GetCourseByIdQueryHandler : IRequestHandler<GetCourseByIdQuery, CourseDetailDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCourseByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CourseDetailDto?> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Courses
            .Include(c => c.Department)
            .AsNoTracking()
            .ProjectTo<CourseDetailDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(c => c.CourseID == request.CourseID, cancellationToken);
    }
}

