using APIKros.Data;
using APIKros.Models;
using APIKros.Requests;
using FluentValidation;

namespace APIKros.Validators;

public abstract class CreateHierarchyNodeValidator<TRequest, TModel>
    : AbstractValidator<TRequest>
    where TRequest : CreateHierarchyNodeRequest
    where TModel : HierarchyNode
{
    protected readonly AppDbContext Context;

    protected CreateHierarchyNodeValidator(AppDbContext context)
    {
        Context = context;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(255).WithMessage("Name must not exceed 255 characters.");
        

        RuleFor(x => x.ManagerId)
            .MustAsync((managerId, ct) =>
                ValidationUtils.EntityExists<Models.Employee>(
                    Context,
                    managerId,
                    ct))
            .WithMessage("Manager does not exist.")
            .When(x => x.ManagerId.HasValue);
    }
    
}


public class CreateCompanyValidator 
    : CreateHierarchyNodeValidator<CreateCompanyRequest, Company>
{
    public CreateCompanyValidator(AppDbContext context) : base(context)
    {
        RuleFor(x => x.Code)
            .MustAsync((code, ct) =>
                ValidationUtils.IsUnique<Company, string>(
                    context,
                    "Code",
                    code!,
                    ct))
            .WithMessage("Company with this Code already exists.");
    }
}