using APIKros.Data;
using APIKros.Requests.Company;
using FluentValidation;

namespace APIKros.Validators.Company
{
    public class UpdateCompanyRequestValidator : AbstractValidator<UpdateCompanyRequest>
    {
        private readonly AppDbContext _context;

        public UpdateCompanyRequestValidator(AppDbContext context)
        {
            _context = context;

        }
    }
}