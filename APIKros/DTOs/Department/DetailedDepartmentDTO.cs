using APIKros.Models;

namespace APIKros.DTOs;

public class DetailedDepartmentDTO   : IDto<Department, DetailedDepartmentDTO>
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Code { get; set; } = "";
    public EmployeeDTO? Manager { get; set; }
    public List<DivisionDTO> Divisions { get; set; } = new();
    public List<EmployeeDTO> Employees { get; set; } = new();

    public static DetailedDepartmentDTO CreateInstance(Department department)
    {
        return new DetailedDepartmentDTO
        {
            Id = department.Id,
            Name = department.Name,
            Code = department.Code,
            Manager = department.Manager is null
                ? null
                : EmployeeDTO.CreateInstance(department.Manager)
        };
    }
}

