using APIKros.Data;
using APIKros.Models;
using APIKros.Requests.Employee;
using FluentValidation;

namespace APIKros.Validators.Employee;

public class ChangeCompanyRequestValidator : AbstractValidator<ChangeCompanyRequest>
{
    private readonly AppDbContext _context;

    public ChangeCompanyRequestValidator(AppDbContext context)
    {
        _context = context;

        RuleFor(x => x.EmployeeId)
            .GreaterThan(0).WithMessage("EmployeeId is required.")
            .MustAsync((employeeId, cancellation) =>
                ValidationUtils.EntityExists<Models.Employee>(
                    _context,
                    employeeId,
                    cancellation))
            .WithMessage("Employee does not exist.");

        RuleFor(x => x.NewCompanyId)
            .GreaterThan(0).WithMessage("NewCompanyId is required.")
            .MustAsync((companyId, cancellation) =>
                ValidationUtils.EntityExists<Models.Company>(
                    _context,
                    companyId,
                    cancellation))
            .WithMessage("Company does not exist.");
    }
}