namespace APIKros.Validators;

public class UpdateEmployeeRequestValidator : AbstractValidator<UpdateEmployeeRequest>
{
    private readonly AppDbContext _context;

    public UpdateEmployeeRequestValidator(AppDbContext context)
    {
        _context = context;

        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email has invalid format.")
            .MaximumLength(255).WithMessage("Email must not exceed 255 characters.")
            .MustAsync(IsUniqueEmail).WithMessage("Employee with this email already exists.");
        
    }
}