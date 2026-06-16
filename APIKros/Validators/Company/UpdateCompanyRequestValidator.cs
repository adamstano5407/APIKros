using FluentValidation;
using APIKRos.Requests.Company;
using APIKRos.Data;
using Microsoft.EntityFrameworkCore;

namespace APIKRos.Validators
{
    public class UpdateCompanyRequestValidator : AbstractValidator<UpdateCompanyRequest>
    {
        private readonly AppDbContext _context;

        public UpdateCompanyRequestValidator(AppDbContext context)
        {
            _context = context;

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.");
        }
    }
}