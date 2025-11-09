using CleanContosoUniversity.Application.Common.Interfaces;
using CleanContosoUniversity.Application.Features.Departments.Queries.Common;

namespace CleanContosoUniversity.Application.Features.Departments.Queries.GetDepartments;

public record GetDepartmentsQuery : IRequest<List<DepartmentDto>?>;

public class GetDepartmentsQueryHandler : IRequestHandler<GetDepartmentsQuery, List<DepartmentDto>?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetDepartmentsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<DepartmentDto>?> Handle(GetDepartmentsQuery request, CancellationToken cancellationToken)
    {
        var departments = await _context.Departments
                .Include(d => d.Administrator)
                .ProjectTo<DepartmentDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

        return departments;
    }
}