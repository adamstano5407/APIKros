using APIKros.Models;
using System.Linq;

namespace APIKros.DTOs
{
    public class DetailedDepartmentDTO : IDto<Department, DetailedDepartmentDTO>
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Code { get; set; } = "";
        public EmployeeDTO? Manager { get; set; }

        public ProjectDTO Project { get; set; } = null!;

        public static DetailedDepartmentDTO CreateInstance(Department department)
        {
            return new DetailedDepartmentDTO
            {
                Id = department.Id,
                Name = department.Name,
                Code = department.Code,
                Manager = department.Manager is null
                    ? null
                    : EmployeeDTO.CreateInstance(department.Manager),
                Project = department.Project is null
                    ? null
                    : ProjectDTO.CreateInstance(department.Project),
            };
        }
    }

}

