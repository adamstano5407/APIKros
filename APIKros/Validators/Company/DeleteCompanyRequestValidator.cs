using FluentValidation;
using APIKRos.Requests.Company;
using APIKRos.Data;
using Microsoft.EntityFrameworkCore;

namespace APIKRos.Validators
{
    public class DeleteCompanyRequestValidator : AbstractValidator<DeleteCompanyRequest>
    {
        private readonly AppDbContext _context;

        public DeleteCompanyRequestValidator(AppDbContext context)
        {
            _context = context;

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.");
        }
    }
}