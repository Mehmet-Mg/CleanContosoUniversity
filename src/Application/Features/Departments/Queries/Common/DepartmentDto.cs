using CleanContosoUniversity.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace CleanContosoUniversity.Application.Features.Departments.Queries.Common;

public class DepartmentDto
{
    public int DepartmentID { get; init; }
    public string Name { get; init; }

    [DataType(DataType.Currency)]
    public decimal Budget { get; init; }

    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",
                   ApplyFormatInEditMode = true)]
    [Display(Name = "Start Date")]
    public DateTime StartDate { get; init; }

    public int? InstructorID { get; init; }

    [Timestamp]
    public byte[]? ConcurrencyToken { get; set; }

    public Instructor? Administrator { get; init; }
    public ICollection<Course> Courses { get; init; } = new List<Course>();

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Department, DepartmentDto>();
        }
    }
}
