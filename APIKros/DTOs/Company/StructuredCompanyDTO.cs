using APIKros.DTOs.Division;
using APIKros.DTOs.Employee;

namespace APIKros.DTOs.Company
{
    public class StructuredCompanyDto : IDto<Models.Company, StructuredCompanyDto>
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public EmployeeDto? Director { get; set; }
        public List<StructuredDivisionDto> Divisions { get; set; } = new();
        public List<EmployeeDto> Employees { get; set; } = new();

        public static StructuredCompanyDto CreateInstance(Models.Company company)
        {
            return new StructuredCompanyDto
            {
                Id = company.Id,
                Name = company.Name,
                Code = company.Code,
                Director = company.Director is null
                    ? null
                    : EmployeeDto.CreateInstance(company.Director),
                Divisions = company.Divisions
                    .Select(StructuredDivisionDto.CreateInstance)
                    .ToList(),
                Employees = company.Employees
                    .Select(EmployeeDto.CreateInstance)
                    .ToList()
            };
        }
    }


}