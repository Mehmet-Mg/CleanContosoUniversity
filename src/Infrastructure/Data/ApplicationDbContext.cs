using CleanContosoUniversity.Application.Common.Interfaces;
using CleanContosoUniversity.Domain.Entities;
using CleanContosoUniversity.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CleanContosoUniversity.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<TodoList> TodoLists => Set<TodoList>();
    public DbSet<TodoItem> TodoItems => Set<TodoItem>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Instructor> Instructors => Set<Instructor>();
    public DbSet<OfficeAssignment> OfficeAssignments => Set<OfficeAssignment>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        builder.Entity<Course>()
            .ToTable(nameof(Course))
            .HasMany(c => c.Instructors)
            .WithMany(i => i.Courses);
        builder.Entity<Student>().ToTable(nameof(Student));
        builder.Entity<Instructor>().ToTable(nameof(Instructor));
        builder.Entity<Department>()
            .Property<byte[]>("ConcurrencyToken")
            .IsRowVersion();
    }
}