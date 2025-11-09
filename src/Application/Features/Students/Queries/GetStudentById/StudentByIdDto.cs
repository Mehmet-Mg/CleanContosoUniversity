using CleanContosoUniversity.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace CleanContosoUniversity.Application.Features.Students.Queries.GetStudentById;

public class StudentByIdDto
{
    public int ID { get; set; }
    [Display(Name = "Last Name")]
    public string LastName { get; set; }
    [Display(Name = "First Name")]
    public string FirstMidName { get; set; }
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [Display(Name = "Enrollment Date")]
    public DateTime EnrollmentDate { get; set; }
    [Display(Name = "Full Name")]
    public string FullName
    {
        get
        {
            return LastName + ", " + FirstMidName;
        }
    }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Student, StudentByIdDto>();
        }
    }
}
