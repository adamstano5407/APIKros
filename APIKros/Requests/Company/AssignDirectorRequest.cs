using System.ComponentModel.DataAnnotations;

namespace APIKros.Requests.Company
{
    public class AssignDirectorRequest
    {
        [Required]
        public int CompanyId { get; set; }
        [Required]
        public int NewDirectorId { get; set; }
    }
    
    
}