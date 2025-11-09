using CleanContosoUniversity.Application.Common.Interfaces;
using CleanContosoUniversity.Application.Common.Mappings;
using CleanContosoUniversity.Application.Common.Models;
using CleanContosoUniversity.Domain.Entities;

namespace CleanContosoUniversity.Application.Features.Students.Queries.GetStudentsWithPagination;

public record GetStudentsWithPaginationQuery : IRequest<PaginatedList<StudentsBriefDto>>
{
    public int PageNumber { get; init; } = 1;
    public int? PageSize { get; init; } = 10;
    public string? SortOrder { get; set; }
    public string? SearchString { get; set; }
    public string? CurrentFilter { get; set; }
}

public class GetStudentsWithPaginationQueryHandler : IRequestHandler<GetStudentsWithPaginationQuery, PaginatedList<StudentsBriefDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetStudentsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<StudentsBriefDto>> Handle(GetStudentsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        int pageIndex = request.PageNumber;
        string searchString = request.SearchString;
        string currentFilter = request.CurrentFilter;

        string nameSort = string.IsNullOrEmpty(request.SortOrder) ? "name_desc" : "";
        string dateSort = request.SortOrder == "Date" ? "date_desc" : "Date";
        if (request.SearchString != null)
        {
            pageIndex = 1;
        }
        else
        {
            searchString = currentFilter;
        }

        currentFilter = searchString;

        IQueryable<Student> studentsIQ = from s in _context.Students
                                         select s;
        if (!string.IsNullOrEmpty(searchString))
        {
            studentsIQ = studentsIQ.Where(s => s.LastName.Contains(searchString)
                                   || s.FirstMidName.Contains(searchString));
        }

        studentsIQ = request.SortOrder switch
        {
            "name_desc" => studentsIQ.OrderByDescending(s => s.LastName),
            "Date" => studentsIQ.OrderBy(s => s.EnrollmentDate),
            "date_desc" => studentsIQ.OrderByDescending(s => s.EnrollmentDate),
            _ => studentsIQ.OrderBy(s => s.LastName),
        };

        var pageSize = request.PageSize ?? 4; // Configuration.GetValue("PageSize", 4);
        //var students = await PaginatedList<Student>.CreateAsync(
        //    studentsIQ.AsNoTracking(), request.PageNumber, pageSize);

        //return await _context.TodoItems
        //    .Where(x => x.ListId == request.ListId)
        //    .OrderBy(x => x.Title)
        //    .ProjectTo<StudentsBriefDto>(_mapper.ConfigurationProvider)
        //    .PaginatedListAsync(request.PageNumber, request.PageSize, cancellationToken);

        return await studentsIQ.AsNoTracking()
                        .ProjectTo<StudentsBriefDto>(_mapper.ConfigurationProvider)
                        .PaginatedListAsync(request.PageNumber, pageSize, cancellationToken);
    }
}