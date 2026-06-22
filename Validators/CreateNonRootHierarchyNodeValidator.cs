using APIKros.Data;
using APIKros.Models;
using APIKros.Requests;
using FluentValidation;

namespace APIKros.Validators;

public abstract class CreateNonRootHierarchyNodeValidator<TRequest, TModel, TParent>
    : CreateHierarchyNodeValidator<TRequest, TModel>
    where TRequest : CreateNonRootHierarchyNodeRequest
    where TModel : HierarchyNode
    where TParent : class
{
    protected CreateNonRootHierarchyNodeValidator(
        AppDbContext context,
        string parentPropertyName)
        : base(context)
    {
        RuleFor(x => x.ParentId)
            .GreaterThan(0).WithMessage("ParentId is required.")
            .MustAsync((parentId, ct) =>
                ValidationUtils.EntityExists<TParent>(context, parentId, ct))
            .WithMessage($"{typeof(TParent).Name} does not exist.");

        RuleFor(x => x.Code)
            .MustAsync((request, code, ct) =>
                ValidationUtils.IsUniqueForParent<TModel>(
                    context,
                    parentPropertyName,
                    request.ParentId,
                    "Code",
                    code!,
                    ct))
            .WithMessage($"{typeof(TModel).Name} with this Code already exists in parent.");
    }
}


public class CreateDivisionRequestValidator
    : CreateNonRootHierarchyNodeValidator<
        CreateDivisionRequest,
        Models.Division,
        Models.Company>
{
    public CreateDivisionRequestValidator(AppDbContext context)
        : base(context, "CompanyId")
    {
    }
}
public class CreateProjectRequestValidator
    : CreateNonRootHierarchyNodeValidator<
        CreateProjectRequest,
        Models.Project,
        Models.Division>
{
    public CreateProjectRequestValidator(AppDbContext context)
        : base(context, "DivisionId")
    {
    }
}
    
public class CreateDepartmentRequestValidator
    : CreateNonRootHierarchyNodeValidator<
        CreateDepartmentRequest,
        Models.Department,
        Models.Project>
{
    public CreateDepartmentRequestValidator(AppDbContext context)
        : base(context, "ProjectId")
    {
    }
}