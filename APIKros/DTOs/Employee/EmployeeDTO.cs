using APIKros.Models;

namespace APIKros.DTOs;

public class EmployeeDTO : IDto<Employee, EmployeeDTO>
{
    public int Id { get; set; }
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Phone { get; set; } = "";
    public int CompanyId { get; set; }

    public static EmployeeDTO CreateInstance(Employee employee)
    {
        return new EmployeeDTO
        {
            Id = employee.Id,
            FullName = $"{employee.Title} {employee.FirstName} {employee.LastName}",
            Email = employee.Email,
            Phone = employee.Phone,
            CompanyId = employee.CompanyId
        };
    }
}