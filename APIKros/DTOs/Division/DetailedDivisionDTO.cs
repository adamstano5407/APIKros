using APIKros.DTOs.Company;
using APIKros.DTOs.Employee;
using APIKros.DTOs.Project;

namespace APIKros.DTOs.Division
{
    public class DetailedDivisionDto : IDto<Models.Division, DetailedDivisionDto>
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Code { get; set; } = "";

        public CompanyDto? Company { get; set; }
        public EmployeeDto? Manager { get; set; }
        public List<ProjectDto> Projects { get; set; } = new();

        public static DetailedDivisionDto CreateInstance(Models.Division division)
        {
            return new DetailedDivisionDto
            {
                Id = division.Id,
                Name = division.Name,
                Code = division.Code,
                Manager = division.Manager is null
                    ? null
                    : EmployeeDto.CreateInstance(division.Manager),
                Projects = division.Projects
                    .Select(ProjectDto.CreateInstance)
                    .ToList(),
                Company = division.Company is null
                    ? null
                    : CompanyDto.CreateInstance(division.Company)
            };
        }
    }
}