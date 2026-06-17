using APIKros.Data;
using APIKros.Requests.Company;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace APIKros.Validators.Company
{

    public class AssignDirectorRequestValidator : AbstractValidator<AssignDirectorRequest>
    {
        private readonly AppDbContext _context;
        
        public AssignDirectorRequestValidator(AppDbContext context)
        {
            _context = context;

            RuleFor(x => x.CompanyId)
                .GreaterThan(0).WithMessage("CompanyId is required.")
                .MustAsync((companyId, cancellation) =>
                    ValidationUtils.EntityExists<Models.Company>(
                        _context,
                        companyId,
                        cancellation))
                .WithMessage("Company does not exist.");

            RuleFor(x => x.NewDirectorId)
                .GreaterThan(0).WithMessage("NewDirectorId is required.")
                .MustAsync((employeeId, cancellation) =>
                    ValidationUtils.EntityExists<Models.Employee>(
                        _context,
                        employeeId,
                        cancellation))
                .WithMessage("Employee does not exist.");
            
            RuleFor(x => x.NewDirectorId)
                .MustAsync(async (request, employeeId, cancellation) =>
                {
                    var employee = await _context.Employees
                        .FirstOrDefaultAsync(e => e.Id == employeeId, cancellation);

                    return employee is not null &&
                           employee.CompanyId == request.CompanyId;
                })
                .WithMessage("Employee must belong to the specified company.");
            
        }
    }    
}

