using System.ComponentModel.DataAnnotations;

namespace APIKros.Requests;

public class AssignManagerRequest
{
    [Required]
    public int EmployeeId { get; set; }

    public int? NodeId { get; set; } = null;
}
