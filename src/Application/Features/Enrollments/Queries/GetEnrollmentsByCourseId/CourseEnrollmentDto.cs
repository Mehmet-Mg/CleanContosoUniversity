using AutoMapper;
using CleanContosoUniversity.Domain.Entities;

namespace CleanContosoUniversity.Application.Features.Enrollments.Queries.GetEnrollmentsByCourseId;

public class CourseEnrollmentDto
{
    public int EnrollmentID { get; set; }
    public int CourseID { get; set; }
    public int StudentID { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public Grade? Grade { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Enrollment, CourseEnrollmentDto>()
                .ForMember(d => d.StudentName, opt => opt.MapFrom(s => s.Student != null ? s.Student.FullName : null));
        }
    }
}

