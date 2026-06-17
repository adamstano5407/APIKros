using APIKros.DTOs.Department;
using APIKros.DTOs.Division;
using APIKros.DTOs.Employee;

namespace APIKros.DTOs.Project
{
    public class DetailedProjectDto : IDto<Models.Project, DetailedProjectDto>
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Code { get; set; } = "";
        public EmployeeDto? Manager { get; set; }
        public DivisionDto? Division { get; set; }

        public List<DepartmentDto> Departments { get; set; } = new();
        public static DetailedProjectDto CreateInstance(Models.Project project)
        {
            return new DetailedProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                Code = project.Code,
                Manager = project.Manager is null
                    ? null
                    : EmployeeDto.CreateInstance(project.Manager),
                Division = DivisionDto.CreateInstance(project.Division),
                Departments = project.Departments.Select(DepartmentDto.CreateInstance).ToList()
            };
        }
    }

}