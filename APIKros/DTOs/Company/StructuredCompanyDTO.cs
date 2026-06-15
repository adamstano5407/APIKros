using APIKros.Models;

namespace APIKros.DTOs;

public class StructuredCompanyDTO : IDto<Company, StructuredCompanyDTO>
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Code { get; set; } = "";
    public EmployeeDTO? Director { get; set; }
    public List<StructuredDivisionDTO> Divisions { get; set; } = new();

    public static StructuredCompanyDTO CreateInstance(Company company)
    {
        return new StructuredCompanyDTO
        {
            Id = company.Id,
            Name = company.Name,
            Code = company.Code,
            Director = company.Director is null
                ? null
                : EmployeeDTO.CreateInstance(company.Director),
            Divisions = company.Divisions
                .Select(StructuredDivisionDTO.CreateInstance)
                .ToList()
        };
    }
}