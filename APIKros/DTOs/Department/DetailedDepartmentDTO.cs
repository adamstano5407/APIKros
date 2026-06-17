using APIKros.DTOs.Employee;
using APIKros.DTOs.Project;

namespace APIKros.DTOs.Department
{
    public class DetailedDepartmentDto : IDto<Models.Department, DetailedDepartmentDto>
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Code { get; set; } = "";
        public EmployeeDto? Manager { get; set; }

        public ProjectDto? Project { get; set; } = null!;

        public static DetailedDepartmentDto CreateInstance(Models.Department department)
        {
            return new DetailedDepartmentDto
            {
                Id = department.Id,
                Name = department.Name,
                Code = department.Code,
                Manager = department.Manager is null
                    ? null
                    : EmployeeDto.CreateInstance(department.Manager),
                Project = department.Project is null
                    ? null
                    : ProjectDto.CreateInstance(department.Project),
            };
        }
    }

}

