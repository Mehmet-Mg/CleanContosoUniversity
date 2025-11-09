using CleanContosoUniversity.Application.Common.Models;
using CleanContosoUniversity.Application.Features.Students.Queries.GetStudentsWithPagination;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CleanContosoUniversity.Web.Pages.Students
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration Configuration;
        private readonly ISender _sender;

        public IndexModel(IConfiguration configuration, ISender sender)
        {
            Configuration = configuration;
            _sender = sender;
        }

        public string NameSort { get; set; }
        public string DateSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public PaginatedList<StudentsBriefDto> Students { get; set; }

        public async Task OnGetAsync( 
            string sortOrder,
            string currentFilter, 
            string searchString, 
            int? pageIndex)
        {
            CurrentSort = sortOrder;
            NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            DateSort = sortOrder == "Date" ? "date_desc" : "Date";
            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            CurrentFilter = searchString;

            var query = new GetStudentsWithPaginationQuery
            {
                CurrentFilter = currentFilter,
                PageSize = Configuration.GetValue("PageSize", 4),
                PageNumber = pageIndex ?? 1,
                SearchString = searchString,
                SortOrder = sortOrder
            };

            Students = await _sender.Send(query);
        }
    }
}
