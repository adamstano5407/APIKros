using APIKros.Data;
using APIKros.Models;
using APIKros.Requests;
using FluentValidation;

namespace APIKros.Validators;

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
                ValidationUtils.IsUnique<Company, string>(
                    _context,
                    "Code",
                    code!,
                    cancellation))
            .WithMessage("Company with this Code already exists.");

        RuleFor(x => x.DirectorId)
            .MustAsync((directorId, cancellation) =>
                ValidationUtils.EntityExists<Employee>(
                    _context,
                    directorId,
                    cancellation))
            .WithMessage("Director does not exist.")
            .When(x => x.DirectorId.HasValue);
    }
}