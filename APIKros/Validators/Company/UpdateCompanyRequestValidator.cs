using FluentValidation;
using APIKros.Data;
using Microsoft.EntityFrameworkCore;
using APIKros.Requests;

namespace APIKros.Validators
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