using AutoMapper;
using CleanContosoUniversity.Domain.Entities;

namespace CleanContosoUniversity.Application.Features.Courses.Queries.GetCourses;

public class CourseDto
{
    public int CourseID { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Credits { get; set; }
    public string DepartmentName { get; set; } = string.Empty;

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Course, CourseDto>()
                .ForMember(d => d.DepartmentName, opt => opt.MapFrom(s => s.Department != null ? s.Department.Name : string.Empty));
        }
    }
}

