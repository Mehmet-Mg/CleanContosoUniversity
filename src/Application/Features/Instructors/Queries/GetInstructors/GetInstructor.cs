using CleanContosoUniversity.Application.Common.Interfaces;

namespace CleanContosoUniversity.Application.Features.Instructors.Queries.GetInstructors;

public record GetInstructorsQuery : IRequest<List<InstructoDto>?>;

public class GetInstructorsQueryHandler : IRequestHandler<GetInstructorsQuery, List<InstructoDto>?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetInstructorsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<InstructoDto>?> Handle(GetInstructorsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Instructors
            .Include(i => i.OfficeAssignment)
            .Include(i => i.Courses)
                .ThenInclude(c => c.Department)
            .AsNoTracking()
            .ProjectTo<InstructoDto>(_mapper.ConfigurationProvider)
            .OrderBy(i => i.LastName)
            .ToListAsync(cancellationToken);
    }
}