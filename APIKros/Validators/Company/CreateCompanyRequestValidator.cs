using APIKros.Data;
using APIKros.Models;
using APIKros.Requests.Company;
using FluentValidation;

namespace APIKros.Validators.Company;

public class CreateCompanyRequestValidator : AbstractValidator<CreateCompanyRequest>
{
    private readonly AppDbContext _context;

    public CreateCompanyRequestValidator(AppDbContext context)
    {
        _context = context;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(255).WithMessage("Name must not exceed 255 characters.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required.")
            .MaximumLength(50).WithMessage("Code must not exceed 50 characters.")
            .MustAsync((code, cancellation) =>
                ValidationUtils.IsUnique<Models.Company, string>(
                    _context,
                    "Code",
                    code!,
                    cancellation))
            .WithMessage("Company with this Code already exists.");

        RuleFor(x => x.DirectorId)
            .MustAsync((directorId, cancellation) =>
                ValidationUtils.EntityExists<Models.Employee>(
                    _context,
                    directorId,
                    cancellation))
            .WithMessage("Director does not exist.")
            .When(x => x.DirectorId.HasValue);
    }
}