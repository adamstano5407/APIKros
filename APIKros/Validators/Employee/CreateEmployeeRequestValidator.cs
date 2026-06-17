using APIKros.Data;
using APIKros.Requests.Employee;
using FluentValidation;

namespace APIKros.Validators.Employee;

public class CreateEmployeeRequestValidator : AbstractValidator<CreateEmployeeRequest>
{
    private readonly AppDbContext _context;

    public CreateEmployeeRequestValidator(AppDbContext context)
    {
        _context = context;

        RuleFor(x => x.Title)
            .MaximumLength(80)
            .Must(ValidationUtils.IsAllowedTitle)
            .WithMessage("Title contains invalid value.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(100).WithMessage("First name must not exceed 100 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(100).WithMessage("Last name must not exceed 100 characters.");

        RuleFor(x => x.Email)
             .NotEmpty()
             .EmailAddress()
             .MustAsync((email, cancellation) =>
                 ValidationUtils.IsUnique<Models.Employee, string>(
                     _context,
                     "Email",
                     email!,
                     cancellation))
             .WithMessage("Employee with this Email already exists.");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone is required.")
            .MaximumLength(30).WithMessage("Phone must not exceed 30 characters.")
            .Must(ValidationUtils.IsValidPhone)
            .WithMessage("Phone must contain only digits, spaces, +, - or /.");

        RuleFor(x => x.CompanyId)
            .NotNull().WithMessage("Company id is required.")
            .GreaterThan(0).WithMessage("Company id must be greater than 0.")
            .MustAsync((companyId, cancellationToken) =>
                ValidationUtils.EntityExists<Models.Company>(_context, companyId, cancellationToken))
            .WithMessage("Company does not exist.");
    }
}