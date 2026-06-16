using APIKros.Models;
using System.Linq;


namespace APIKros.DTOs
{
    public class DetailedDivisionDTO : IDto<Division, DetailedDivisionDTO>
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Code { get; set; } = "";

        public CompanyDTO? Company { get; set; }
        public EmployeeDTO? Manager { get; set; }
        public List<ProjectDTO> Projects { get; set; } = new();

        public static DetailedDivisionDTO CreateInstance(Division division)
        {
            return new DetailedDivisionDTO
            {
                Id = division.Id,
                Name = division.Name,
                Code = division.Code,
                Manager = division.Manager is null
                    ? null
                    : EmployeeDTO.CreateInstance(division.Manager),
                Projects = division.Projects
                    .Select(ProjectDTO.CreateInstance)
                    .ToList(),
                Company = division.Company is null
                    ? null
                    : CompanyDTO.CreateInstance(division.Company)
            };
        }
    }
}