using CleanContosoUniversity.Application.Features.Courses.Queries.GetCoursesForInstructor;
using CleanContosoUniversity.Web.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CleanContosoUniversity.Web.Pages.Instructors;

public class InstructorCoursesPageModel : PageModel
{
    public List<AssignedCourseData> AssignedCourseDataList { get; set; } = new();

    public void PopulateAssignedCourseData(List<CourseForInstructorDto> allCourses, int[] instructorCourses)
    {
        var instructorCoursesHS = new HashSet<int>(instructorCourses);
        AssignedCourseDataList = new List<AssignedCourseData>();
        
        foreach (var course in allCourses)
        {
            AssignedCourseDataList.Add(new AssignedCourseData
            {
                CourseID = course.CourseID,
                Title = course.Title,
                Assigned = instructorCoursesHS.Contains(course.CourseID)
            });
        }
    }
}
