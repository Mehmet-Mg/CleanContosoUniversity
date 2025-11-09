using CleanContosoUniversity.Domain.Entities;

namespace CleanContosoUniversity.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoList> TodoLists { get; }
    DbSet<TodoItem> TodoItems { get; }
    DbSet<Student> Students { get; }
    DbSet<Department> Departments { get; }
    DbSet<Instructor> Instructors { get; }
    DbSet<Course> Courses { get; }
    DbSet<Enrollment> Enrollments { get; }
    DbSet<OfficeAssignment> OfficeAssignments { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}