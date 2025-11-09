namespace CleanContosoUniversity.Application.Features.Students.Queries.GetStudentByIdWithDetails;

internal class GetStudentByIdWithDetailsQueryValidator : AbstractValidator<GetStudentByIdWithDetailsQuery>
{
    public GetStudentByIdWithDetailsQueryValidator()
    {
        RuleFor(x => x.StudentId)
            .NotEmpty().NotNull().WithMessage("StudentId is required.");

        //RuleFor(x => x.PageNumber)
        //    .GreaterThanOrEqualTo(1).WithMessage("PageNumber at least greater than or equal to 1.");

        //RuleFor(x => x.PageSize)
        //    .GreaterThanOrEqualTo(1).WithMessage("PageSize at least greater than or equal to 1.");
    }
}