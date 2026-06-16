using APIKros.Models;
using System.Linq;


namespace APIKros.DTOs
{
    public class DetailedProjectDTO : IDto<Project, DetailedProjectDTO>
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Code { get; set; } = "";
        public EmployeeDTO? Manager { get; set; }
        public DivisionDTO? Division { get; set; }

        public List<DepartmentDTO> Departments { get; set; } = new();
        public static DetailedProjectDTO CreateInstance(Project project)
        {
            return new DetailedProjectDTO
            {
                Id = project.Id,
                Name = project.Name,
                Code = project.Code,
                Manager = project.Manager is null
                    ? null
                    : EmployeeDTO.CreateInstance(project.Manager),
                Division = project.Division is null ? null : DivisionDTO.CreateInstance(project.Division),
                Departments = project.Departments.Select(DepartmentDTO.CreateInstance).ToList()
            };
        }
    }

}