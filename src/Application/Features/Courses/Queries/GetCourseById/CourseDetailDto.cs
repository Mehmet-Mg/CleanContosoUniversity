using AutoMapper;
using CleanContosoUniversity.Domain.Entities;

namespace CleanContosoUniversity.Application.Features.Courses.Queries.GetCourseById;

public class CourseDetailDto
{
    public int CourseID { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Credits { get; set; }
    public int DepartmentID { get; set; }
    public string? DepartmentName { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Course, CourseDetailDto>()
                .ForMember(d => d.DepartmentName, opt => opt.MapFrom(s => s.Department != null ? s.Department.Name : null));
        }
    }
}

