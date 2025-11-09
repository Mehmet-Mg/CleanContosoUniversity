using CleanContosoUniversity.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CleanContosoUniversity.Web.Pages
{
    public class AboutModel : PageModel
    {

        private readonly ApplicationDbContext _context;

        public AboutModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public class EnrollmentDateGroup
        {
            [DataType(DataType.Date)]
            public DateTime? EnrollmentDate { get; set; }

            public int StudentCount { get; set; }
        }

        public IList<EnrollmentDateGroup> Students { get; set; }

        public async Task OnGetAsync()
        {
            IQueryable<EnrollmentDateGroup> data =
                from student in _context.Students
                group student by student.EnrollmentDate into dateGroup
                select new EnrollmentDateGroup()
                {
                    EnrollmentDate = dateGroup.Key,
                    StudentCount = dateGroup.Count()
                };

            Students = await data.AsNoTracking().ToListAsync();
        }
    }
}
