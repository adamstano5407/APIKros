using APIKros.Models;
using System.Linq;

namespace APIKros.DTOs
{
    public class DetailedCompanyDTO : IDto<Company, DetailedCompanyDTO>
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Code { get; set; } = "";
        public EmployeeDTO? Director { get; set; }
        public List<DivisionDTO> Divisions { get; set; } = new();
        public List<EmployeeDTO> Employees { get; set; } = new();

        public static DetailedCompanyDTO CreateInstance(Company company)
        {
            return new DetailedCompanyDTO
            {
                Id = company.Id,
                Name = company.Name,
                Code = company.Code,
                Director = company.Director is null
                    ? null
                    : EmployeeDTO.CreateInstance(company.Director),
                Divisions = company.Divisions
                    .Select(DivisionDTO.CreateInstance)
                    .ToList(),
                Employees = company.Employees
                    .Select(EmployeeDTO.CreateInstance)
                    .ToList()
            };
        }
    }

}