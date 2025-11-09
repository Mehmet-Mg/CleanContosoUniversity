using CleanContosoUniversity.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanContosoUniversity.Domain.Entities;

public class Department : BaseEntity
{
    public int DepartmentID { get; set; }

    [StringLength(50, MinimumLength = 3)]
    public string Name { get; set; }

    [DataType(DataType.Currency)]
    [Column(TypeName = "money")]
    public decimal Budget { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",
                   ApplyFormatInEditMode = true)]
    [Display(Name = "Start Date")]
    public DateTime StartDate { get; set; }

    public int? InstructorID { get; set; }

    [Timestamp]
    public byte[] ConcurrencyToken { get; set; }

    public Instructor? Administrator { get; set; }
    public ICollection<Course> Courses { get; set; } = new List<Course>();
}
