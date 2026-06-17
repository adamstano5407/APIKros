namespace APIKros.DTOs.Project;

public class ProjectDto : IDto<Models.Project, ProjectDto>
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Code { get; set; } = "";
    public int DivisionId { get; set; }
    public int? ManagerId { get; set; }

    public static ProjectDto CreateInstance(Models.Project project)
    {
        return new ProjectDto
        {
            Id = project.Id,
            Name = project.Name,
            Code = project.Code,
            DivisionId = project.DivisionId,
            ManagerId = project.ManagerId
        };
    }
}