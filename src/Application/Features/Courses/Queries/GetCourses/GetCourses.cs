using CleanContosoUniversity.Application.Common.Interfaces;

namespace CleanContosoUniversity.Application.Features.Courses.Queries.GetCourses;

public record GetCoursesQuery : IRequest<List<CourseDto>>;

public class GetCoursesQueryHandler : IRequestHandler<GetCoursesQuery, List<CourseDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCoursesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<CourseDto>> Handle(GetCoursesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Courses
            .Include(c => c.Department)
            .AsNoTracking()
            .ProjectTo<CourseDto>(_mapper.ConfigurationProvider)
            .OrderBy(c => c.Title)
            .ToListAsync(cancellationToken);
    }
}

