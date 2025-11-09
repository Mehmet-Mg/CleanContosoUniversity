using CleanContosoUniversity.Application.Common.Interfaces;
using FluentValidation;

namespace CleanContosoUniversity.Application.Features.Students.Commands.UpdateStudent;

public class UpdateStudentCommandValidator : AbstractValidator<UpdateStudentCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateStudentCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("Student ID is required.");

        RuleFor(v => v.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50).WithMessage("Last name must not exceed 50 characters.");

        RuleFor(v => v.FirstMidName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50).WithMessage("First name must not exceed 50 characters.");

        RuleFor(v => v.EnrollmentDate)
            .NotEmpty().WithMessage("Enrollment date is required.");
    }
}
