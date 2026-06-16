using APIKros.Models;
using System.Linq;


namespace APIKros.DTOs;

public class EmployeeDTO : IDto<Employee, EmployeeDTO>
{
    public int Id { get; set; }

    public string? Title { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public int CompanyId { get; set; } 

    public static EmployeeDTO CreateInstance(Employee employee)
    {
        return new EmployeeDTO
        {
            Id = employee.Id,
            Title = employee.Title,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Email = employee.Email,
            Phone = employee.Phone,
            CompanyId = employee.CompanyId
        };
    }
}