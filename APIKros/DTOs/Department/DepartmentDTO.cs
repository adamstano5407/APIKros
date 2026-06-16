using APIKros.Models;
using System.Linq;

namespace APIKros.DTOs
{
    public class DepartmentDTO : IDto<Department, DepartmentDTO>
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Code { get; set; } = "";
        public int ProjectId { get; set; }
        public int? ManagerId { get; set; }

        public static DepartmentDTO CreateInstance(Department department)
        {
            return new DepartmentDTO
            {
                Id = department.Id,
                Name = department.Name,
                Code = department.Code,
                ProjectId = department.ProjectId,
                ManagerId = department.ManagerId
            };
        }
    }
}