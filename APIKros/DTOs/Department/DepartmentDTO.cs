namespace APIKros.DTOs.Department
{
    public class DepartmentDto : IDto<Models.Department, DepartmentDto>
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Code { get; set; } = "";
        public int ProjectId { get; set; }
        public int? ManagerId { get; set; }

        public static DepartmentDto CreateInstance(Models.Department department)
        {
            return new DepartmentDto
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