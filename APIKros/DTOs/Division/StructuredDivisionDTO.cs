using APIKros.Models;
using System.Linq;


namespace APIKros.DTOs
{

    public class StructuredDivisionDTO : IDto<Division, StructuredDivisionDTO>
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Code { get; set; } = "";
        public CompanyDTO Company { get; set; }
        public EmployeeDTO? Manager { get; set; }

        public List<DetailedProjectDTO> Projects { get; set; } = new();
        public static StructuredDivisionDTO CreateInstance(Division division)
        {
            return new StructuredDivisionDTO
            {
                Id = division.Id,
                Name = division.Name,
                Code = division.Code,
                Company = CompanyDTO.CreateInstance(division.Company),
                Manager = division.Manager is null
                    ? null
                    : EmployeeDTO.CreateInstance(division.Manager),
                Projects = division.Projects is null
                    ? null
                    : division.Projects.Select(DetailedProjectDTO.CreateInstance).ToList()
            };
        }
    }
}
