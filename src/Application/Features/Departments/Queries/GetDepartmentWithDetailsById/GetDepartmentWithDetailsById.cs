using CleanContosoUniversity.Application.Common.Interfaces;
using CleanContosoUniversity.Application.Features.Departments.Queries.Common;

namespace CleanContosoUniversity.Application.Features.Departments.Queries.GetDepartmentWithDetailsById;

public record GetDepartmentWithDetailByIdQuery : IRequest<DepartmentDto?>
{
    public int DepartmentId { get; init; }
}

public class GetDepartmentWithDetailByIdQueryhandler : IRequestHandler<GetDepartmentWithDetailByIdQuery, DepartmentDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetDepartmentWithDetailByIdQueryhandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<DepartmentDto?> Handle(GetDepartmentWithDetailByIdQuery request, CancellationToken cancellationToken)
    {
        var department = await _context.Departments
            .Include(d => d.Administrator)
            .ProjectTo<DepartmentDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(m => m.DepartmentID == request.DepartmentId);

        return department;
    }
}