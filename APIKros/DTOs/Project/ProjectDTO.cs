using APIKros.Models;
using System.Linq;


namespace APIKros.DTOs;

public class ProjectDTO : IDto<Project, ProjectDTO>
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Code { get; set; } = "";
    public int DivisionId { get; set; }
    public int? ManagerId { get; set; }

    public static ProjectDTO CreateInstance(Project project)
    {
        return new ProjectDTO
        {
            Id = project.Id,
            Name = project.Name,
            Code = project.Code,
            DivisionId = project.DivisionId,
            ManagerId = project.ManagerId
        };
    }
}