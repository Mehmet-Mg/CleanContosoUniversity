using CleanContosoUniversity.Application.Common.Interfaces;
using FluentValidation;

namespace CleanContosoUniversity.Application.Features.Courses.Commands.CreateCourse;

public class CreateCourseCommandValidator : AbstractValidator<CreateCourseCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateCourseCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.CourseID)
            .NotEmpty().WithMessage("Course ID is required.");

        RuleFor(v => v.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MinimumLength(3).WithMessage("Title must be at least 3 characters.")
            .MaximumLength(50).WithMessage("Title must not exceed 50 characters.");

        RuleFor(v => v.Credits)
            .NotEmpty().WithMessage("Credits is required.")
            .InclusiveBetween(0, 5).WithMessage("Credits must be between 0 and 5.");

        RuleFor(v => v.DepartmentID)
            .NotEmpty().WithMessage("Department is required.")
            .MustAsync(DepartmentExists)
                .WithMessage("Selected department does not exist.");
    }

    private async Task<bool> DepartmentExists(int departmentId, CancellationToken cancellationToken)
    {
        return await _context.Departments
            .AnyAsync(d => d.DepartmentID == departmentId, cancellationToken);
    }
}

