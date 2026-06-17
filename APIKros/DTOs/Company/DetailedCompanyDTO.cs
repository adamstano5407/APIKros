using APIKros.DTOs.Division;
using APIKros.DTOs.Employee;

namespace APIKros.DTOs.Company
{
    public class DetailedCompanyDto : IDto<Models.Company, DetailedCompanyDto>
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Code { get; set; } = "";
        public EmployeeDto? Director { get; set; }
        public List<DivisionDto> Divisions { get; set; } = new();
        public List<EmployeeDto> Employees { get; set; } = new();

        public static DetailedCompanyDto CreateInstance(Models.Company company)
        {
            return new DetailedCompanyDto
            {
                Id = company.Id,
                Name = company.Name,
                Code = company.Code,
                Director = company.Director is null
                    ? null
                    : EmployeeDto.CreateInstance(company.Director),
                Divisions = company.Divisions
                    .Select(DivisionDto.CreateInstance)
                    .ToList(),
                Employees = company.Employees
                    .Select(EmployeeDto.CreateInstance)
                    .ToList()
            };
        }
    }

}