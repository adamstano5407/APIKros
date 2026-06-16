using APIKros.Data;
using APIKros.Models;
using APIKros.Requests;
using FluentValidation;

namespace APIKros.Validators;

public class UpdateEmployeeRequestValidator : AbstractValidator<UpdateEmployeeRequest>
{
    private readonly AppDbContext _context;

    public UpdateEmployeeRequestValidator(AppDbContext context)
    {
        _context = context;

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Email has invalid format.")
            .MaximumLength(255).WithMessage("Email must not exceed 255 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.CompanyId)
            .MustAsync((companyId, cancellation) =>
                ValidationUtils.EntityExists<Company>(
                    _context,
                    companyId!.Value,
                    cancellation))
            .WithMessage("Company does not exist.")
            .When(x => x.CompanyId.HasValue);
    }
}