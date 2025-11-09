using CleanContosoUniversity.Application.Common.Interfaces;
using FluentValidation;

namespace CleanContosoUniversity.Application.Features.Instructors.Commands.UpdateInstructor;

public class UpdateInstructorCommandValidator : AbstractValidator<UpdateInstructorCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateInstructorCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.ID)
            .NotEmpty().WithMessage("Instructor ID is required.");

        RuleFor(v => v.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50).WithMessage("Last name must not exceed 50 characters.");

        RuleFor(v => v.FirstMidName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50).WithMessage("First name must not exceed 50 characters.");

        RuleFor(v => v.HireDate)
            .NotEmpty().WithMessage("Hire date is required.")
            .LessThanOrEqualTo(DateTime.Today).WithMessage("Hire date cannot be in the future.");

        RuleFor(v => v.OfficeLocation)
            .MaximumLength(50).WithMessage("Office location must not exceed 50 characters.")
            .When(v => !string.IsNullOrWhiteSpace(v.OfficeLocation));
    }
}

