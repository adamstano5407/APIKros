using FluentValidation;
using APIKRos.Requests.Company;
using APIKRos.Data;
using Microsoft.EntityFrameworkCore;

namespace APIKRos.Validators
{
    public class CreateCompanyRequestValidator : AbstractValidator<CreateCompanyRequest>
    {
        private readonly AppDbContext _context;

        public CreateCompanyRequestValidator(AppDbContext context)
        {
            _context = context;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100);

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Code is required.")
                .MustAsync(async (code, cancellation) =>
                    !await _context.Companies.AnyAsync(c => c.Code == code, cancellation))
                .WithMessage("Company with this Code already exists.");
        }
    }
}