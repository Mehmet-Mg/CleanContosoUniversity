using CleanContosoUniversity.Application.Features.Departments.Commands.UpdateDepartment;
using CleanContosoUniversity.Application.Features.Departments.Queries.GetDepartmentWithDetailsById;
using CleanContosoUniversity.Application.Features.Instructors.Queries.GetInstructorById;
using CleanContosoUniversity.Application.Features.Departments.Queries.Common;
using CleanContosoUniversity.Application.Features.Instructors.Queries.GetInstructors;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CleanContosoUniversity.Web.Pages.Departments;

public class EditModel : PageModel
{
    private readonly ISender _sender;

    public EditModel(ISender sender)
    {
        _sender = sender;
    }

    [BindProperty]
    public DepartmentDto? Department { get; set; }
    public SelectList InstructorNameSL { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Department = await _sender.Send(new GetDepartmentWithDetailByIdQuery
        {
            DepartmentId = id
        });

        if (Department == null)
        {
            return NotFound();
        }

        var instructors = await _sender.Send(new GetInstructorsQuery());
        InstructorNameSL = new SelectList(instructors ?? new List<InstructoDto>(),
            "ID", "FullName", Department.InstructorID);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        if (!ModelState.IsValid)
        {
            var instructors = await _sender.Send(new GetInstructorsQuery());
            InstructorNameSL = new SelectList(instructors ?? [],
                "ID", "FullName", Department?.InstructorID);
            return Page();
        }

        var departmentToUpdate = await _sender.Send(new GetDepartmentWithDetailByIdQuery
        {
            DepartmentId = id
        });

        if (departmentToUpdate == null)
        {
            return await HandleDeletedDepartment();
        }

        if (await TryUpdateModelAsync<DepartmentDto>(
            departmentToUpdate,
            "Department",
            s => s.Name, s => s.StartDate, s => s.Budget, s => s.InstructorID))
        {
            try
            {
                await _sender.Send(new UpdateDepartmentCommand
                {
                    Budget = departmentToUpdate.Budget,
                    DepartmentId = departmentToUpdate.DepartmentID,
                    InstructorID = departmentToUpdate.InstructorID,
                    Name = departmentToUpdate.Name,
                    StartDate = departmentToUpdate.StartDate,
                    ConcurrencyToken = departmentToUpdate.ConcurrencyToken ?? Array.Empty<byte>()
                });
                return RedirectToPage("./Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                // Get current values from database
                var dbValues = await _sender.Send(new GetDepartmentWithDetailByIdQuery
                {
                    DepartmentId = id
                });

                if (dbValues == null)
                {
                    ModelState.AddModelError(string.Empty, "Unable to save. " +
                        "The department was deleted by another user.");
                    var instructors = await _sender.Send(new GetInstructorsQuery());
                    InstructorNameSL = new SelectList(instructors ?? [],
                        "ID", "FullName", Department?.InstructorID);
                    return Page();
                }

                await SetDbErrorMessage(dbValues, departmentToUpdate);

                Department.ConcurrencyToken = dbValues.ConcurrencyToken;
                ModelState.Remove($"{nameof(Department)}.{nameof(Department.ConcurrencyToken)}");
            }
        }

        var instructorsList = await _sender.Send(new GetInstructorsQuery());
        InstructorNameSL = new SelectList(instructorsList ?? [],
            "ID", "FullName", departmentToUpdate.InstructorID);

        return Page();
    }

    private async Task<IActionResult> HandleDeletedDepartment()
    {
        ModelState.AddModelError(string.Empty,
            "Unable to save. The department was deleted by another user.");
        
        var instructors = await _sender.Send(new GetInstructorsQuery());
        InstructorNameSL = new SelectList(instructors ?? [],
            "ID", "FullName", Department?.InstructorID);
        return Page();
    }

    private async Task SetDbErrorMessage(DepartmentDto dbValues, DepartmentDto clientValues)
    {
        if (dbValues.Name != clientValues.Name)
        {
            ModelState.AddModelError("Department.Name",
                $"Current value: {dbValues.Name}");
        }
        if (dbValues.Budget != clientValues.Budget)
        {
            ModelState.AddModelError("Department.Budget",
                $"Current value: {dbValues.Budget:c}");
        }
        if (dbValues.StartDate != clientValues.StartDate)
        {
            ModelState.AddModelError("Department.StartDate",
                $"Current value: {dbValues.StartDate:d}");
        }
        if (dbValues.InstructorID != clientValues.InstructorID)
        {
            var dbInstructor = await _sender.Send(new GetInstructorByIdQuery
            {
                InstructorID = dbValues.InstructorID
            });

            ModelState.AddModelError("Department.InstructorID",
                $"Current value: {dbInstructor?.FullName}");
        }

        ModelState.AddModelError(string.Empty,
            "The record you attempted to edit "
          + "was modified by another user after you. The "
          + "edit operation was canceled and the current values in the database "
          + "have been displayed. If you still want to edit this record, click "
          + "the Save button again.");
    }
}
